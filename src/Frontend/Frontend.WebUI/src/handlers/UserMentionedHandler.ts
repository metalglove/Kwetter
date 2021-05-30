import { EventCallBack, EventHandler } from '@/interfaces/INotificationService';
import UserMentionedEvent from '@/models/events/UserMentionedEvent';
import { ElMessage } from 'element-plus';

export default class UserMentionedHandler extends EventHandler<UserMentionedEvent> {
    public callback: EventCallBack<UserMentionedEvent>;

    constructor() {
        super('UserMentionedIntegrationEvent');
        this.callback = this.userMentioned;
    }

    private userMentioned(data: UserMentionedEvent): void {
        console.log('UserMentionedEvent', data);
        ElMessage({
            message: 'You were mentioned in a kweet!',
            type: 'info'
        });
    }
}