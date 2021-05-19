import firebase from 'firebase/app';
import 'firebase/auth';
import { EventHandler, EventHandlerDestructor, INotificationService } from '@/interfaces/INotificationService';
import BaseEvent from '@/models/events/BaseEvent';
import KwetterErrorEvent from '@/models/events/KwetterErrorEvent';
import KwetterErrorHandler from '@/handlers/KwetterErrorHandler';
import UserUnauthorizedHandler from '../handlers/UserUnauthorizedHandler';
import { toUserFromIdToken, User } from '@/modules/User/User';

/**
 * Represents the NotificationService class.
 */
export default class NotificationService implements INotificationService {
    private _notificationPath: string = '/Notification/';
    private _eventHandlers: Map<string, EventHandler<any>[]>;
    private webSocket: WebSocket | undefined;

    /**
     * Initializes a new instance of the NotificationService class.
     */
    constructor(gatewayWsApiUrl: string) {
        this._eventHandlers = new Map<string, EventHandler<any>[]>();
        this.addEventHandler(new KwetterErrorHandler());
        this.addEventHandler(new UserUnauthorizedHandler());

        firebase.auth().onAuthStateChanged(async (user) => {
            if (user) {
                const authUser: User = toUserFromIdToken(await user.getIdToken());
                if (authUser?.userId) {
                    this.webSocket = new WebSocket(`${gatewayWsApiUrl + this._notificationPath}Live/`);
                    this.webSocket.onopen = (_) => this.openHandler(user);
                    this.webSocket.onmessage = this.messageHandler.bind(this);
                    this.webSocket.onclose = this.closeHandler.bind(this);
                    this.webSocket.onerror = this.errorHandler.bind(this);
                }
            } else {
                this.webSocket?.close();
            }
        });
    }

    public addEventHandler<TEvent extends BaseEvent>(eventHandler: EventHandler<TEvent>): void {
        if (this._eventHandlers.has(eventHandler.eventName)) {
            this._eventHandlers.get(eventHandler.eventName)!.push(eventHandler);
        } else {
            this._eventHandlers.set(eventHandler.eventName, [eventHandler]);
        }
    }

    public removeEventHandler<TEvent extends BaseEvent>(eventHandler: EventHandler<TEvent>): void {
        if (this._eventHandlers.has(eventHandler.eventName)) {
            const filteredEventHandlers: EventHandler<any>[] = this._eventHandlers
                .get(eventHandler.eventName)!
                .filter((handler) => handler !== eventHandler);
            this._eventHandlers.set(eventHandler.eventName, filteredEventHandlers);
        }
    }

    public eventHandlerDestructor(): EventHandlerDestructor {
        return (eventHandler: EventHandler<any>) => this.removeEventHandler(eventHandler);
    }

    private messageHandler(message: MessageEvent): void {
        const baseEvent: BaseEvent = JSON.parse(message.data) as BaseEvent;
        if (!this._eventHandlers.has(baseEvent.EventName)) {
            console.log(`No handler found for ${baseEvent.EventName}`);
            return;
        }
        this._eventHandlers.get(baseEvent.EventName)!
            .forEach((eventHandler) => {
                eventHandler.callback(message.data);
            });
    }

    private errorHandler(event: Event): void {
        this._eventHandlers.get('ErrorHandler')!.forEach((eventHandler) => {
            eventHandler.callback({ Message: 'Could not connect to the notification server.' } as KwetterErrorEvent);
        });
    }

    private async openHandler(user: firebase.User): Promise<void> {
        const token: string = await user.getIdToken(true);
        this.webSocket!.send(token);
    }

    private closeHandler(event: CloseEvent): void {
        console.log('Closed!', event);
    }
}