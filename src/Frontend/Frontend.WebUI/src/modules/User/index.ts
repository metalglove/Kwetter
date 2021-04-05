import { Store, CommitOptions, DispatchOptions, Module } from 'vuex';
import { RootState } from '@/modules';
import { UserActions, userActions } from './User.actions';
import { UserGetters, userGetters } from './User.getters';
import { UserMutations, userMutations } from './User.mutations';
import { IUserState as State, userState } from './User.state';

export type IUserState = State;

export type UserModule = {
    namespaced?: boolean;
    state?: IUserState;
    mutations?: UserMutations;
    actions?: UserActions;
    getters?: UserGetters;
};

export type UserStore<S = IUserState> = Omit<Store<S>, 'getters' | 'commit' | 'dispatch'>
    & {
        commit<K extends keyof UserMutations, P extends Parameters<UserMutations[K]>[1]>(
            key: K,
            payload: P,
            options?: CommitOptions
        ): ReturnType<UserMutations[K]>;
    } & {
        dispatch<K extends keyof UserActions>(
            key: K,
            payload: Parameters<UserActions[K]>[1],
            options?: DispatchOptions
        ): ReturnType<UserActions[K]>;
    } & {
        getters: {
            [K in keyof UserGetters]: ReturnType<UserGetters[K]>
        };
    };

export const userStore: Module<IUserState, RootState> & UserModule = {
    namespaced: true,
    state: userState,
    mutations: userMutations,
    actions: userActions,
    getters: userGetters
};