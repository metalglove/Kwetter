import BaseEvent from './BaseEvent';

export default interface UserFollowedEvent extends BaseEvent {
    FollowingId: string;
    FollowerId: string;
    FollowedDateTime: string;
}