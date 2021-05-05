import CommandResponse from '@/models/cqrs/CommandResponse';
import QueryResponse from '@/models/cqrs/QueryResponse';
import VerifyUserNameUniquenessDto from '@/models/dtos/VerifyUserNameUniquenessDto';

/**
 * Represents the IAuthorizationService interface.
 */
export default interface IAuthorizationService {
    /**
     * Request claims for a new user from the authorization service.
     * @param idToken The id token.
     * @param userName The user name.
     */
    SetClaims(idToken: string, userName: string): Promise<CommandResponse>;

    /**
     * Verifies the user name uniqueness from the authorization service.
     * @param userName The user name.
     */
    VerifyUserNameUniqueness(userName: string): Promise<QueryResponse<VerifyUserNameUniquenessDto>>
}
