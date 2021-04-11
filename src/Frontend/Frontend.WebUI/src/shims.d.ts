import { Store } from 'vuex';
import IAuthorizationService from '@/interfaces/IAuthorizationService';
import IKweetService from '@/interfaces/IKweetService';
import { RootState } from '@/modules';

declare module '@vue/runtime-core' {
    interface ComponentCustomProperties {
        $store: Store<RootState>;
        $authorizationService: IAuthorizationService;
        $kweetService: IKweetService;
    }
}