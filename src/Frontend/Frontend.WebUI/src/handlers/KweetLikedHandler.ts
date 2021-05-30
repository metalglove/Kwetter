import { EventCallBack, EventHandler } from '@/interfaces/INotificationService';
import { ElMessage } from 'element-plus';
import KweetLikedEvent from '../models/events/KweetLikedEvent';

export default class KweetLikedHandler extends EventHandler<KweetLikedEvent> {
    public callback: EventCallBack<KweetLikedEvent>;

    constructor() {
        super('KweetLikedIntegrationEvent');
        this.callback = this.kweetLiked;
    }

    private kweetLiked(data: KweetLikedEvent): void {
        console.log('KweetLikedEvent', data);
        ElMessage({
            message: 'Your kweet was liked!',
            type: 'info'
        });
    }
}