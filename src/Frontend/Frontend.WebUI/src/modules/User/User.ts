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
    expires_in: number,
    first_issued_at: number,
    idpId: string,
    scope: string,
    login_hint: string,
    reloadAuthResponse(): any
};

export function toUser(currentUser: any): User {
    const basicProfile = currentUser.getBasicProfile();
    const authResponse = currentUser.getAuthResponse();
    const user: User = {
        userId: currentUser.getId(),
        profile: {
            name: basicProfile.getName(),
            picture: basicProfile.getImageUrl(),
            email: basicProfile.getEmail()
        },
        authentication: {
            token_type: authResponse.token_type,
            access_token: authResponse.access_token,
            expires_at: authResponse.expires_at,
            expires_in: authResponse.expires_in,
            first_issued_at: authResponse.first_issued_at,
            idpId: authResponse.idpId,
            id_token: authResponse.id_token,
            login_hint: authResponse.login_hint,
            scope: authResponse.scope,
            reloadAuthResponse: () => currentUser.reloadAuthResponse()
        }
    }
    return user;
}