import { createStore } from 'vuex';
import { IAppState, AppStore, appStore } from './App'
import { IUserState, UserStore, userStore } from './User';

export interface RootState {
    app: IAppState;
    user: IUserState;
}

export type RootStore = AppStore<Pick<RootState, 'app'>>
    & UserStore<Pick<RootState, 'user'>>;

export const rootStore = createStore({
    modules: {
        app: appStore,
        user: userStore
    }
});

export function useStore() {
    return rootStore as RootStore
};