import jwtDecode, { JwtPayload } from 'jwt-decode';

export type User = {
    userId: string,
    profile: UserProfile,
};

export type UserProfile = {
    email: string,
    picture: string,
    name: string
};

export function toUserFromIdToken(idToken: string): User {
    const payload: GoogleJwtPayload = jwtDecode<GoogleJwtPayload>(idToken);
    const user: User = {
        userId: payload.UserId,
        profile: {
            name: payload.name,
            picture: payload.picture,
            email: payload.email
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