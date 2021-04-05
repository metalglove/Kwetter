import { Store, CommitOptions, DispatchOptions, Module } from 'vuex';
import { RootState } from '@/modules';
import { AppActions, appActions } from './App.actions';
import { AppGetters, appGetters } from './App.getters';
import { AppMutations, appMutations } from './App.mutations';
import { IAppState as State, appState } from './App.state';

export type IAppState = State;

export type AppModule = {
    namespaced?: boolean;
    state?: IAppState;
    mutations?: AppMutations;
    actions?: AppActions;
    getters?: AppGetters;
};

export type AppStore<S = IAppState> = Omit<Store<S>, 'getters' | 'commit' | 'dispatch'>
    & {
        commit<K extends keyof AppMutations, P extends Parameters<AppMutations[K]>[1]>(
            key: K,
            payload: P,
            options?: CommitOptions
        ): ReturnType<AppMutations[K]>;
    } & {
        dispatch<K extends keyof AppActions>(
            key: K,
            payload: Parameters<AppActions[K]>[1],
            options?: DispatchOptions
        ): ReturnType<AppActions[K]>;
    } & {
        getters: {
            [K in keyof AppGetters]: ReturnType<AppGetters[K]>
        };
    };

export const appStore: Module<IAppState, RootState> & AppModule = {
    namespaced: true,
    state: appState,
    mutations: appMutations,
    actions: appActions,
    getters: appGetters
};