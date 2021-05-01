import QueryResponse from '@/models/cqrs/QueryResponse';
import { Kweet } from '@/modules/Kweet/Kweet';
import CommandResponse from '@/models/cqrs/CommandResponse';

/**
 * Represents the IKweetService interface.
 */
export default interface IKweetService {
    /**
     * Posts a kweet.
     * @param kweetId The kweet id.
     * @param message The message.
     * @param userId The user id.
     */
    postKweet(kweetId: string, message: string, userId: string): Promise<CommandResponse>;

    /**
     * Likes a kweet.
     * @param kweetId The kweet id.
     * @param userId The user id.
     */
    likeKweet(kweetId: string, userId: string): Promise<CommandResponse>;

    /**
     * Unlikes a kweet.
     * @param kweetId The kweet id.
     * @param userId The user id.
     */
    unlikeKweet(kweetId: string, userId: string): Promise<CommandResponse>;

    /**
     * Paginates the kweets by user id.
     * @param pageNumber The page number.
     * @param pageSize The page size.
     * @param userId The user id.
     * @returns A query response containing the paginated kweets.
     */
    paginateKweets(pageNumber: number, pageSize: number, userId: string): Promise<QueryResponse<Kweet[]>>;
}