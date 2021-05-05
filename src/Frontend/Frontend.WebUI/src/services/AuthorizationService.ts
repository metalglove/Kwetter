import IAuthorizationService from '@/interfaces/IAuthorizationService';
import IHttpCommunicator from '@/interfaces/IHttpCommunicator';
import ClaimsCommand from '@/models/cqrs/Authorization/ClaimsCommand';
import CommandResponse from '@/models/cqrs/CommandResponse';
import VerifyUserNameUniquenessQuery from '@/models/cqrs/Authorization/VerifyUserNameUniquenessQuery';
import QueryResponse from '@/models/cqrs/QueryResponse';
import VerifyUserNameUniquenessDto from '@/models/dtos/VerifyUserNameUniquenessDto';

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

    public async VerifyUserNameUniqueness(userName: string): Promise<QueryResponse<VerifyUserNameUniquenessDto>> {
        this.userNameValidator(userName);

        const verifyUserNameUniquenessQuery: VerifyUserNameUniquenessQuery = {
            UserName: userName
        };

        return await this._httpCommunicator.post<VerifyUserNameUniquenessQuery, QueryResponse<VerifyUserNameUniquenessDto>>(`${this._authorizationPath}VerifyUserNameUniqueness`, verifyUserNameUniquenessQuery);
    }

    public async SetClaims(idToken: string, userName: string): Promise<CommandResponse> {
        if (idToken.length == 0)
            throw new Error('The id token is empty.');
        this.userNameValidator(userName);

        const claimsCommand: ClaimsCommand = {
            IdToken: idToken,
            UserName: userName
        };

        return await this._httpCommunicator.post<ClaimsCommand, CommandResponse>(`${this._authorizationPath}Claims`, claimsCommand);
    }

    private userNameValidator(userName: string): boolean {
        if (userName.length == 0)
            throw new Error('The user name can not be empty.');
        if (userName.length > 32)
            throw new Error('The user name length may not exceed 32 characters.');
        if (userName.length < 3)
            throw new Error('The user name length should be at least 3 characters.');
        if (!userName.match(/^[A-Za-z0-9]+$/))
            throw new Error('The user name is not alphanumeric.');
        return true;
    }
}
