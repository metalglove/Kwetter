import IAuthorizationService from '@/interfaces/IAuthorizationService';
import IHttpCommunicator from '@/interfaces/IHttpCommunicator';
import ClaimsCommand from '@/models/cqrs/Authorization/ClaimsCommand';
import CommandResponse from '@/models/cqrs/CommandResponse';

/**
 * Represents the AuthorizationService class.
 */
export default class AuthorizationService implements IAuthorizationService {
    private _authorizationPath: string = '/Authorization/';
    private _httpCommunicator: IHttpCommunicator;

    /**
     * Initializes a new instance of the AuthorizationService class.
     */
    constructor(httpCommunicator: IHttpCommunicator) {
        this._httpCommunicator = httpCommunicator;
    }

    public async SetClaims(idToken: string): Promise<CommandResponse> {
        if (idToken.length == 0)
            throw new Error('The id token is empty.');

        const claimsCommand: ClaimsCommand = {
            IdToken: idToken
        };

        return await this._httpCommunicator.post<ClaimsCommand, CommandResponse>(`${this._authorizationPath}Claims`, claimsCommand);
    }
}
