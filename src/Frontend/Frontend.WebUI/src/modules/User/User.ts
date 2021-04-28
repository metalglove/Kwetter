import jwtDecode, { JwtPayload } from "jwt-decode";

export type User = {
    userId: string,
    profile: UserProfile,
    authentication: UserAuthentication
};

export type UserProfile = {
    email: string,
    picture: string,
    name: string
};

export type UserAuthentication = {
    token_type: string,
    id_token: string,
    expires_at: number,
    expires_in: number
};

export function toUserFromIdToken(idToken: string): User {
    const payload: GoogleJwtPayload = jwtDecode<GoogleJwtPayload>(idToken);
    const user: User = {
        userId: payload.UserId,
        profile: {
            name: payload.name,
            picture: payload.picture,
            email: payload.email
        },
        authentication: {
            token_type: 'Bearer',
            expires_at: payload.iat,
            expires_in: payload.exp,
            id_token: idToken
        }
    }
    return user;
}

interface GoogleJwtPayload extends JwtPayload {
    email: string;
    email_verified: boolean;
    azp: string;
    at_hash: string;
    name: string;
    picture: string;
    given_name: string;
    locale: string;
    iat: number;
    exp: number;
    UserId: string;
    User: boolean;
}