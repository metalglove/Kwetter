import IHttpCommunicator from "@/interfaces/IHttpCommunicator";
import { User } from "@/modules/User/User";
import { getItem } from "./LocalStorageUtilities";

/**
 * Represents the HttpCommunicator class.
 * Used to communicate with external HTTP endpoints.
 */
export default class HttpCommunicator implements IHttpCommunicator {
    private _gatewayUrl: String;

    /**
     * Initializes a new instance of the HttpCommunicator class.
     */
    constructor(gatewayUrl: string) {
        this._gatewayUrl = gatewayUrl;
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
            headers: this.getHeaders()
        };
        if (body)
            request.body = JSON.stringify(body);
        const response = await fetch(`${this._gatewayUrl}${path}`, request);
        return await response.json() as TResponse;
    }

    private getHeaders(): Headers {
        const headers: Headers = new Headers();
        headers.append('Content-Type', 'application/json');
        const user: User | null = getItem<User>('user');
        if (user)
            headers.append('Authorization', `Bearer ${user.authentication.id_token}`);
        return headers;
    }
}