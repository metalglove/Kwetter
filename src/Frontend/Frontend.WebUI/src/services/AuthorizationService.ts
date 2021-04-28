import IAuthorizationService from '@/interfaces/IAuthorizationService';
import ClaimsResponse from '@/models/dtos/ClaimsResponse';
import QueryResponse from '@/models/cqrs/QueryResponse';

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

    public async RequestClaimsForNewUser(idToken: string): Promise<QueryResponse<ClaimsResponse>> {
         const response = await fetch(
            `${this._authorizationServiceUri}/Claims`,
            {
                method: 'POST',
                cache: 'no-cache',
                headers: {
                    'Content-Type': 'application/json'
                },
                redirect: 'follow',
                body: JSON.stringify({ IdToken: idToken })
            });
        if (!response.ok) {
            const message = `An error has occured: ${response.status}`;
            throw new Error(message);
        }
        const queryResponse: QueryResponse<ClaimsResponse> = await response.json();
        return queryResponse;
    }
}
