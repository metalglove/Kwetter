//import { ComponentCustomProperties } from 'vue';
import { Store } from 'vuex';
import { RootState } from './modules';

declare module '@vue/runtime-core' {
    interface ComponentCustomProperties {
        $store: Store<RootState>
    }
}