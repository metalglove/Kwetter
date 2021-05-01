import QueryResponse from '@/models/cqrs/QueryResponse';
import { Kweet } from '@/modules/Kweet/Kweet';
import IHttpCommunicator from '@/interfaces/IHttpCommunicator';
import ITimelineService from '../interfaces/ITimelineService';

/**
 * Represents the TimelineService class.
 */
export default class TimelineService implements ITimelineService {
    private _httpCommunicator: IHttpCommunicator;
    private _timelinePath: string = '/Timeline/';

    /**
     * Initializes a new instance of the TimelineService class.
     */
    constructor(httpCommunicator: IHttpCommunicator) {
        this._httpCommunicator = httpCommunicator;
    }

    public paginateKweets(pageNumber: number, pageSize: number): Promise<QueryResponse<Kweet[]>> {
        if (pageNumber < 0)
            throw new Error('The page number can not be negative.');
        if (pageSize < 5)
            throw new Error('The page size can not be smaller than 5.');

        return this._httpCommunicator.get<QueryResponse<Kweet[]>>(`${this._timelinePath}Paginate?pageNumber=${pageNumber}&pageSize=${pageSize}`);
    }
}