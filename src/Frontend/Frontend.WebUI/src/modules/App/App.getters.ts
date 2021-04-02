import { GetterTree } from 'vuex'
import { AppState } from './App.state'
import { RootState } from '@/modules';

export enum AppGetterTypes {
    GET_DRAWER = 'GET_DRAWER'
}

export type AppGetters = {
    [AppGetterTypes.GET_DRAWER](state: AppState): boolean | null;
}

export const appGetters: GetterTree<AppState, RootState> & AppGetters = {
    [AppGetterTypes.GET_DRAWER](state: AppState): boolean | null {
        return state.drawer;
    }
}