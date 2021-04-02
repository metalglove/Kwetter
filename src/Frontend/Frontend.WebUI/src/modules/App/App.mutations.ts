import { MutationTree } from 'vuex';
import { AppState } from './App.state';

export enum AppMutationTypes {
    SET_DRAWER = 'SET_DRAWER',
    TOGGLE_DRAWER = 'TOGGLE_DRAWER'
}

export type AppMutations = {
    [AppMutationTypes.SET_DRAWER](state: AppState, drawer: boolean | null): void;
    [AppMutationTypes.TOGGLE_DRAWER](state: AppState): void;
}

export const appMutations: MutationTree<AppState> & AppMutations = {
    [AppMutationTypes.SET_DRAWER](state: AppState, drawer: boolean | null): void {
        state.drawer = drawer;
    },
    [AppMutationTypes.TOGGLE_DRAWER](state: AppState): void {
        state.drawer = !state.drawer;
    }
}