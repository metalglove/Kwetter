/**
 * Represents the IHttpCommunicator interface.
 */
export default interface IHttpCommunicator {
    /**
     * Performs a GET request.
     * @param path The path.
     */
    get<TResponse>(path: string): Promise<TResponse>;

    /**
     * Performs a GET request.
     * @param path The path.
     * @param body The body.
     */
    getWithBody<TRequest, TResponse>(path: string, body: TRequest): Promise<TResponse>;

    /**
     * Performs a POST request.
     * @param path The path.
     * @param body The body.
     */
    post<TRequest, TResponse>(path: string, body: TRequest): Promise<TResponse>;

    /**
     * Performs a PUT request.
     * @param path The path.
     * @param body The body.
     */
    put<TRequest, TResponse>(path: string, body: TRequest): Promise<TResponse>;

    /**
     * Performs a DELETE request.
     * @param path The path.
     * @param body The body.
     */
    delete<TRequest, TResponse>(path: string, body: TRequest): Promise<TResponse>;
}
