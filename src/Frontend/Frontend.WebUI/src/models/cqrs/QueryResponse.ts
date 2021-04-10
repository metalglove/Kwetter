import Response from "./Response";

/**
 * Represents the QueryResponse interface.
 * Holds the default properties for responses and data of T.
 */
export default interface QueryResponse<T> extends Response {
    data: T;
}