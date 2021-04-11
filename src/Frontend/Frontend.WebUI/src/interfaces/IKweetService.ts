import QueryResponse from "@/models/cqrs/QueryResponse";
import Response from "@/models/cqrs/Response";
import { Kweet } from "@/modules/Kweet/Kweet";

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
    postKweet(kweetId: string, message: string, userId: string): Promise<Response>;

    /**
     * Likes a kweet.
     * @param kweetId The kweet id.
     * @param userId The user id.
     */
    likeKweet(kweetId: string, userId: string): Promise<Response>;

    /**
     * Unlikes a kweet.
     * @param kweetId The kweet id.
     * @param userId The user id.
     */
    unlikeKweet(kweetId: string, userId: string): Promise<Response>;

    /**
     * Paginates the kweets by user id.
     * @param pageNumber The page number.
     * @param pageSize The page size.
     * @param userId The user id.
     * @returns A query response containing the paginated kweets.
     */
    paginateKweets(pageNumber: number, pageSize: number, userId: string): Promise<QueryResponse<Kweet[]>>;
}