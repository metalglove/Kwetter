/**
 * Represents the ClaimsResponse interface.
 */
export default interface ClaimsResponse {
  claims: string[];
  user_id: string;
  token_id: string;
}

/**
 * Represents the Claim interface.
 */
export default interface Claim {
  type: string;
  value: string;
}
