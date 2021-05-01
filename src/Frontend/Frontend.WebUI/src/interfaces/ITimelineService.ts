import QueryResponse from '@/models/cqrs/QueryResponse';
import { Kweet } from '@/modules/Kweet/Kweet';

/**
 * Represents the ITimelineService interface.
 */
export default interface ITimelineService {
    /**
     * Paginates the kweets by user id.
     * @param pageNumber The page number.
     * @param pageSize The page size.
     * @returns A query response containing the paginated kweets.
     */
    paginateKweets(pageNumber: number, pageSize: number): Promise<QueryResponse<Kweet[]>>;
}
