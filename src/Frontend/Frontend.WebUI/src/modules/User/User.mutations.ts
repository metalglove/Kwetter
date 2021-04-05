import { MutationTree } from 'vuex';
import { User } from './User';
import { IUserState } from './User.state';

export enum UserMutationTypes {
    SET_USER = 'SET_USER'
}

export type UserMutations = {
    [UserMutationTypes.SET_USER](state: IUserState, user: User): void;
}

export const userMutations: MutationTree<IUserState> & UserMutations = {
    [UserMutationTypes.SET_USER](state: IUserState, user: User): void {
        state.user = user;
    }
};