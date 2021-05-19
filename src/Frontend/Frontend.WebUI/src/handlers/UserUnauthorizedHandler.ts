import { EventCallBack, EventHandler } from '@/interfaces/INotificationService';
import UserUnauthorizedEvent from '@/models/events/UserUnauthorizedEvent';

export default class UserUnauthorizedHandler extends EventHandler<UserUnauthorizedEvent> {
    public callback: EventCallBack<UserUnauthorizedEvent>;

    constructor() {
        super('UserUnauthorizedEvent');
        this.callback = this.userUnauthorized;
    }

    private userUnauthorized(data: UserUnauthorizedEvent): void {
        console.log('UserUnauthorizedEvent', data);
    }
}