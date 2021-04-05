import { MutationTree } from 'vuex';
import { IAppState } from './App.state';

export enum AppMutationTypes {
    SET_DRAWER = 'SET_DRAWER',
    TOGGLE_DRAWER = 'TOGGLE_DRAWER'
}

export type AppMutations = {
    [AppMutationTypes.SET_DRAWER](state: IAppState, drawer: boolean | null): void;
    [AppMutationTypes.TOGGLE_DRAWER](state: IAppState): void;
}

export const appMutations: MutationTree<IAppState> & AppMutations = {
    [AppMutationTypes.SET_DRAWER](state: IAppState, drawer: boolean | null): void {
        state.drawer = drawer;
    },
    [AppMutationTypes.TOGGLE_DRAWER](state: IAppState): void {
        state.drawer = !state.drawer;
    }
}