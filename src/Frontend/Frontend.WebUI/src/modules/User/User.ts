import AuthorizationDto from "../../models/dtos/AuthorizationDto";
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
    access_token: string,
    id_token: string,
    expires_at: number,
    expires_in: number
};

export function toUser(authorizationDto: AuthorizationDto): User {
    const payload: GoogleJwtPayload = jwtDecode<GoogleJwtPayload>(authorizationDto.id_token);
    const user: User = {
        userId: authorizationDto.user_id,
        profile: {
            name: payload.name,
            picture: payload.picture,
            email: payload.email
        },
        authentication: {
            token_type: authorizationDto.token_type,
            access_token: authorizationDto.access_token,
            expires_at: payload.iat,
            expires_in: payload.exp,
            id_token: authorizationDto.id_token
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
}