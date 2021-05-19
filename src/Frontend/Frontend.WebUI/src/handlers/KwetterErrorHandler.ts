import { EventCallBack, EventHandler } from '@/interfaces/INotificationService';
import KwetterErrorEvent from '@/models/events/KwetterErrorEvent';

export default class KwetterErrorHandler extends EventHandler<KwetterErrorEvent> {
    public callback: EventCallBack<KwetterErrorEvent>;

    constructor() {
        super('ErrorHandler');
        this.callback = this.error;
    }

    private error(data: KwetterErrorEvent): void {
        console.log('KwetterErrorEvent', data);
    }
}