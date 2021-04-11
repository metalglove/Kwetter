import IKweetService from '@/interfaces/IKweetService';
import QueryResponse from '@/models/cqrs/QueryResponse';
import Response from '@/models/cqrs/Response';
import { Kweet } from '@/modules/Kweet/Kweet';
import IHttpCommunicator from '@/interfaces/IHttpCommunicator';
import Guid from '@/utils/Guid';
import CreateKweetCommand from '@/models/cqrs/Kweet/CreateKweetCommand';
import LikeKweetCommand from '../models/cqrs/Kweet/LikeKweetCommand';

/**
 * Represents the KweetService class.
 */
export default class KweetService implements IKweetService {
    private _httpCommunicator: IHttpCommunicator;
    private _kweetPath: string = '/Kweet/';

    /**
     * Initializes a new instance of the KweetService class.
     */
    constructor(httpCommunicator: IHttpCommunicator) {
        this._httpCommunicator = httpCommunicator;
    }

    public postKweet(kweetId: string, message: string, userId: string): Promise<Response> {
        if (!Guid.isValid(kweetId))
            throw new Error('Invalid kweet id.');
        if (!Guid.isValid(userId))
            throw new Error('Invalid user id.');
        if (message.length < 3)
            throw new Error('The message needs to have at least 3 characters.');
        if (message.length > 140)
            throw new Error('The message exceeded the maximum message length of 140.');

        const command: CreateKweetCommand = {
            KweetId: kweetId,
            UserId: userId,
            Message: message
        };

        return this._httpCommunicator.post<CreateKweetCommand, Response>(`${this._kweetPath}`, command);
    }

    public async likeKweet(kweetId: string, userId: string): Promise<Response> {
        if (!Guid.isValid(kweetId))
            throw new Error('Invalid kweet id.');
        if (!Guid.isValid(userId))
            throw new Error('Invalid user id.');

        const command: LikeKweetCommand = {
            UserId: userId,
            KweetId: kweetId
        };

        return this._httpCommunicator.post<LikeKweetCommand, Response>(`${this._kweetPath}Like`, command);
    }

    public async unlikeKweet(kweetId: string, userId: string): Promise<Response> {
        if (!Guid.isValid(kweetId))
            throw new Error('Invalid kweet id.');
        if (!Guid.isValid(userId))
            throw new Error('Invalid user id.');

        const command: LikeKweetCommand = {
            UserId: userId,
            KweetId: kweetId
        };

        return this._httpCommunicator.delete<LikeKweetCommand, Response>(`${this._kweetPath}Like`, command);
    }

    public async paginateKweets(pageNumber: number, pageSize: number, userId: string): Promise<QueryResponse<Kweet[]>> {
        throw new Error('Method not implemented.');
    }
}