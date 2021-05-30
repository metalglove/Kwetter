import { EventCallBack, EventHandler } from '@/interfaces/INotificationService';
import { ElMessage } from 'element-plus';
import UserFollowedEvent from '../models/events/UserFollowedEvent';

export default class UserFollowedHandler extends EventHandler<UserFollowedEvent> {
    public callback: EventCallBack<UserFollowedEvent>;

    constructor() {
        super('UserFollowedIntegrationEvent');
        this.callback = this.userFollowed;
    }

    private userFollowed(data: UserFollowedEvent): void {
        console.log('UserFollowedEvent', data);
        ElMessage({
            message: 'You have a new follower!',
            type: 'info'
        });
    }
}