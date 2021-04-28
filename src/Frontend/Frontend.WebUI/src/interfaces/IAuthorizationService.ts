import QueryResponse from "@/models/cqrs/QueryResponse";
import ClaimsResponse from "@/models/dtos/ClaimsResponse";

/**
 * Represents the IAuthorizationService interface.
 */
export default interface IAuthorizationService {
    /**
     * Request claims for a new user from the authorization service.
     * @param idToken The id token.
     */
    RequestClaimsForNewUser(idToken: string): Promise<QueryResponse<ClaimsResponse>>;
}
