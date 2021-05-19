import BaseEvent from '@/models/events/BaseEvent';

export interface INotificationService {
    addEventHandler<TEvent extends BaseEvent>(eventHandler: EventHandler<TEvent>): void;
    removeEventHandler<TEvent extends BaseEvent>(eventHandler: EventHandler<TEvent>): void;
    eventHandlerDestructor(): EventHandlerDestructor;
}

export type EventCallBack<TEvent extends BaseEvent> = (data: TEvent) => void;
export type EventHandlerDestructor = (data: EventHandler<any>) => void;

export abstract class EventHandler<TEvent extends BaseEvent> {
    public eventName!: string;
    public abstract callback: EventCallBack<TEvent>;

    constructor(eventName: string) {
        this.eventName = eventName;
    }
}