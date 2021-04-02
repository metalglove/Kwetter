import { createStore } from 'vuex';
import { AppStore, appStore } from './App'
import { AppState } from './App/App.state';

export interface RootState {
    app: AppState
}

export type RootStore = AppStore<Pick<RootState, 'app'>>;
    //& ProfileStore<Pick<RootState, 'profile'>>;

export const rootStore = createStore({
    modules: {
        app: appStore
    }
})

export function useStore() {
    return rootStore as RootStore
}