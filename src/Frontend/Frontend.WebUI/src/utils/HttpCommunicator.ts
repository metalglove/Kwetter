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

    async get(path: string): Promise<any> {
        return await this.fetchAsync('GET', path);
    }

    async post(path: string, body: any): Promise<any> {
        return await this.fetchAsync('POST', path, body);
    }

    async put(path: string, body: any): Promise<any> {
        return await this.fetchAsync('PUT', path, body);
    }

    async delete(path: string, body: any): Promise<any> {
        return await this.fetchAsync('DELETE', path, body);
    }

    private async fetchAsync(method: string, path: string, body?: any): Promise<any> {
        const request: RequestInit = {
            method: method,
            headers: this.getHeaders()
        };

        if (body)
            request.body = JSON.stringify(body);

        return await fetch(`${this._gatewayUrl}${path}`, request);
    }

    private getHeaders(): Headers {
        const headers: Headers = new Headers();
        headers.append('Content-Type', 'application/json');
        const user: User | null = getItem<User>('user');
        if (user)
            headers.append('Authorization', `Bearer ${user.authentication.access_token}`);
        return headers;
    }
}