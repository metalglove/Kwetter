import BaseEvent from './BaseEvent';

export default interface UserUnauthorizedEvent extends BaseEvent {
    Message: string;
}