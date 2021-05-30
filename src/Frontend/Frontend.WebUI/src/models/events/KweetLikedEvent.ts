import BaseEvent from './BaseEvent';

export default interface KweetLikedEvent extends BaseEvent {
    KweetId: string;
    KweetUserId: string;
    UserId: string;
    LikedDateTime: string;
}