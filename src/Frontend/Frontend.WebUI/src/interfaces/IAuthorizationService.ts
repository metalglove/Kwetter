import QueryResponse from "@/models/cqrs/QueryResponse";
import AuthorizationDto from "@/models/dtos/AuthorizationDto";

/**
 * Represents the IAuthorizationService interface.
 */
export default interface IAuthorizationService {
    /**
     * Authorizes the client using the authorization_code flow.
     * @param code The authorization code.
     */
    AuthorizeCode(code: string): Promise<QueryResponse<AuthorizationDto>>;
}
