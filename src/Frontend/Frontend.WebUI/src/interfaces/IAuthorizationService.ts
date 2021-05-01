import CommandResponse from "@/models/cqrs/CommandResponse";
import ClaimsCommand from "../models/cqrs/Authorization/ClaimsCommand";

/**
 * Represents the IAuthorizationService interface.
 */
export default interface IAuthorizationService {
    /**
     * Request claims for a new user from the authorization service.
     * @param idToken The id token.
     */
    SetClaims(idToken: string): Promise<CommandResponse>;
}
