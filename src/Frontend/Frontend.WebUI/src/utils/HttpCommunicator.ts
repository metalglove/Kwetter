import IHttpCommunicator from '@/interfaces/IHttpCommunicator';
import firebase from 'firebase/app';
import 'firebase/auth';
import Response from '@/models/cqrs/Response';
import serviceUnreachableResponse from '@/models/cqrs/ServiceUnreachableResponse';

/**
 * Represents the HttpCommunicator class.
 * Used to communicate with external HTTP endpoints.
 */
export default class HttpCommunicator implements IHttpCommunicator {
    private _gatewayUrl: String;
    private _firebaseApp: firebase.app.App;

    /**
     * Initializes a new instance of the HttpCommunicator class.
     */
    constructor(gatewayUrl: string, firebaseApp: firebase.app.App) {
        this._gatewayUrl = gatewayUrl;
        this._firebaseApp = firebaseApp;
    }

    get<TResponse extends Response>(path: string): Promise<TResponse> {
        return this.fetchAsync('GET', path);
    }

    getWithBody<TRequest, TResponse extends Response>(path: string, body: TRequest): Promise<TResponse> {
        return this.fetchAsync('GET', path, body);
    }

    post<TRequest, TResponse extends Response>(path: string, body: TRequest): Promise<TResponse> {
        return this.fetchAsync('POST', path, body);
    }

    put<TRequest, TResponse extends Response>(path: string, body: TRequest): Promise<TResponse> {
        return this.fetchAsync('PUT', path, body);
    }

    delete<TRequest, TResponse extends Response>(path: string, body: TRequest): Promise<TResponse> {
        return this.fetchAsync('DELETE', path, body);
    }

    private async fetchAsync<TRequest, TResponse extends Response>(method: string, path: string, body?: TRequest | unknown): Promise<TResponse> {
        const request: RequestInit = {
            method: method,
            headers: {
                'Content-Type': `application/json`,
                'Authorization': `Bearer ${await this._firebaseApp.auth().currentUser?.getIdToken()}`
            }
        };
        if (body)
            request.body = JSON.stringify(body);
        return fetch(`${this._gatewayUrl}${path}`, request).then(async (response) => {
            return await response.json() as TResponse;
        }, (error) => {
            return serviceUnreachableResponse as TResponse;
        });
    }
}