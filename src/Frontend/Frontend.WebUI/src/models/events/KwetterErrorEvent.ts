import BaseEvent from './BaseEvent';

export default interface KwetterErrorEvent extends BaseEvent {
    Message: string;
}