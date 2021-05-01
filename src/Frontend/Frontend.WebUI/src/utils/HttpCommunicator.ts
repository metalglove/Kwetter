import IHttpCommunicator from '@/interfaces/IHttpCommunicator';
import firebase from 'firebase/app';
import 'firebase/auth';

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

    get<TResponse>(path: string): Promise<TResponse> {
        return this.fetchAsync('GET', path);
    }

    getWithBody<TRequest, TResponse>(path: string, body: TRequest): Promise<TResponse> {
        return this.fetchAsync('GET', path, body);
    }

    post<TRequest, TResponse>(path: string, body: TRequest): Promise<TResponse> {
        return this.fetchAsync('POST', path, body);
    }

    put<TRequest, TResponse>(path: string, body: TRequest): Promise<TResponse> {
        return this.fetchAsync('PUT', path, body);
    }

    delete<TRequest, TResponse>(path: string, body: TRequest): Promise<TResponse> {
        return this.fetchAsync('DELETE', path, body);
    }

    private async fetchAsync<TRequest, TResponse>(method: string, path: string, body?: TRequest | unknown): Promise<TResponse> {
        const request: RequestInit = {
            method: method,
            headers: await this.getHeaders()
        };
        if (body)
            request.body = JSON.stringify(body);
        const response = await fetch(`${this._gatewayUrl}${path}`, request);
        return await response.json() as TResponse;
    }

    private async getHeaders(): Promise<Headers> {
        const headers: Headers = new Headers();
        headers.append('Content-Type', 'application/json');
        const firebaseUser: firebase.User | null = this._firebaseApp.auth().currentUser;
        if (firebaseUser)
            headers.append('Authorization', `Bearer ${await firebaseUser.getIdToken()}`);
        return headers;
    }
}