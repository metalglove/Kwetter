/**
 * Represents the Kweet type.
 * */
export type Kweet = {
    id: string,
    userId: string,
    userDisplayName: string,
    liked: boolean,
    likeCount: number,
    message: string,
    userProfilePictureUrl: string,
    createdDateTime: string
};