import IAuthorizationService from '@/interfaces/IAuthorizationService';
import QueryResponse from '../models/cqrs/QueryResponse';
import AuthorizationDto from '../models/dtos/AuthorizationDto';

/**
 * Represents the AuthorizationService class.
 */
export default class AuthorizationService implements IAuthorizationService {
    private _authorizationServiceUri: String;

    /**
     * Initializes a new instance of the AuthorizationService class.
     */
    constructor(authorizationServiceUri: String) {
        this._authorizationServiceUri = authorizationServiceUri;
    }

    public async AuthorizeCode(code: string): Promise<QueryResponse<AuthorizationDto>> {
        const response = await fetch(
            `${this._authorizationServiceUri}/Code`,
            {
                method: 'POST',
                cache: 'no-cache',
                headers: {
                    'Content-Type': 'application/json'
                },
                redirect: 'follow',
                body: JSON.stringify({ code: code })
            });
        if (!response.ok) {
            const message = `An error has occured: ${response.status}`;
            throw new Error(message);
        }
        const queryResponse: QueryResponse<AuthorizationDto> = await response.json();
        return queryResponse;
    }
}
