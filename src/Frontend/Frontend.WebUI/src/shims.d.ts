import { Store } from 'vuex';
import IAuthorizationService from '@/interfaces/IAuthorizationService';
import IKweetService from '@/interfaces/IKweetService';
import { RootState } from '@/modules';
import firebase from 'firebase/app';
import ITimelineService from './interfaces/ITimelineService';

declare module '@vue/runtime-core' {
    interface ComponentCustomProperties {
        $store: Store<RootState>;
        $authorizationService: IAuthorizationService;
        $kweetService: IKweetService;
        $timelineService: ITimelineService;
        $firebaseAuth: firebase.app.App;
    }
}