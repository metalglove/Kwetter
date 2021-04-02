import { ActionTree, ActionContext } from 'vuex';
import { AppState } from './App.state';
import { AppMutations, AppMutationTypes } from './App.mutations';
import { RootState } from '@/modules';

export enum AppActionTypes {
    SET_DRAWER = 'SET_DRAWER',
    TOGGLE_DRAWER = 'TOGGLE_DRAWER'
}

type AugmentedAppActionContext = {
    commit<K extends keyof AppMutations>(
        key: K,
        payload: Parameters<AppMutations[K]>[1]
    ): ReturnType<AppMutations[K]>
} & Omit<ActionContext<AppState, RootState>, 'commit'>

export interface AppActions {
    [AppActionTypes.SET_DRAWER](
        { commit }: AugmentedAppActionContext,
        payload: boolean
    ): void;
    [AppActionTypes.TOGGLE_DRAWER](
        { commit }: AugmentedAppActionContext,
        payload: undefined
    ): void;
}

export const appActions: ActionTree<AppState, RootState> & AppActions = {
    [AppActionTypes.SET_DRAWER](
        { commit }: AugmentedAppActionContext,
        payload: boolean
    ): void {
        commit(AppMutationTypes.SET_DRAWER, payload);
    },
    [AppActionTypes.TOGGLE_DRAWER](
        { commit }: AugmentedAppActionContext,
        payload: undefined
    ): void {
        commit(AppMutationTypes.TOGGLE_DRAWER, payload);
    }
}