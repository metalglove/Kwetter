/**
 * Represents the IHttpCommunicator interface.
 */
export default interface IHttpCommunicator {
    /**
     * Performs a GET request.
     * @param path The path.
     */
    get(path: string): Promise<any>;

    /**
     * Performs a POST request.
     * @param path The path.
     * @param body The body.
     */
    post(path: string, body: any): Promise<any>;

    /**
     * Performs a PUT request.
     * @param path The path.
     * @param body The body.
     */
    put(path: string, body: any): Promise<any>;

    /**
     * Performs a DELETE request.
     * @param path The path.
     * @param body The body.
     */
    delete(path: string, body: any): Promise<any>;
}
