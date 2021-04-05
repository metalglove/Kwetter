import { GetterTree } from 'vuex';
import { IAppState } from './App.state';
import { RootState } from '@/modules';

export enum AppGetterTypes {
    GET_DRAWER = 'GET_DRAWER'
}

export type AppGetters = {
    [AppGetterTypes.GET_DRAWER](state: IAppState): boolean | null;
}

export const appGetters: GetterTree<IAppState, RootState> & AppGetters = {
    [AppGetterTypes.GET_DRAWER](state: IAppState): boolean | null {
        return state.drawer;
    }
}