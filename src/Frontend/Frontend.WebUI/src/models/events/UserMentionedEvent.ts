import BaseEvent from './BaseEvent';

export default interface UserMentionedEvent extends BaseEvent {
    KweetId: string;
    UserId: string;
    MentionedByUserId: string;
    UserName: string;
    MentionedDateTime: string;
}