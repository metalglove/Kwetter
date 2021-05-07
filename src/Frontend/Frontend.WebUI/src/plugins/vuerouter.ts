import { createRouter, createWebHashHistory, Router, RouteRecordRaw } from 'vue-router';
import { User, toUserFromIdToken } from '@/modules/User/User';
import firebase from 'firebase/app';
import 'firebase/auth';

export type KwetterRoute = {
    name: string,
    props?: Record<string, any>
}

async function isLoggedIn(): Promise<boolean> {
    try {
        await new Promise((resolve, reject) =>
            firebase.auth().onAuthStateChanged(
                user => {
                    if (user) {
                        // User is signed in.
                        resolve(user);
                    } else {
                        // No user is signed in.
                        reject('no user logged in');
                    }
                },
                // Prevent console error
                error => reject(error)
            )
        )
        return true;
    } catch (error) {
        return false;
    }
}

export function createKwetterRouter(routes: KwetterRoute[]): Router {
    /* tslint:disable */
    const loadView = (name: string): any => import(`../views/${name}.vue`);
    /* tslint:enable */

    const router: Router = createRouter({
        history: createWebHashHistory(),
        routes: []
    });

    router.beforeEach(async (to, from, next) => {
        const publicViews = ['/', '/Home', '/Register'];
        const authRequired = !publicViews.includes(to.path);
        if (!authRequired)
            return next();
        const loggedIn = await isLoggedIn();
        // If not logged in, redirect the user to the home view.
        if (!loggedIn) {
            return next('/Home');
        }
        const token = await firebase.auth().currentUser!.getIdToken();
        const user: User = toUserFromIdToken(token);
        if (user.userId == null) {
            return next('/Register');
        }
        return next();
    });

    routes.forEach(route => {
        const view: string = route.name.replace(/^\w/, (c) => c.toUpperCase());
        const routeRecordedRaw: RouteRecordRaw = { path: `/${route.name}`, name: route.name, component: () => loadView(view), props: route.props };
        router.addRoute(routeRecordedRaw);
    });
    router.addRoute({ path: '/', redirect: '/Home' });

    // Redirect for unknown routes.
    router.addRoute({ path: '/:pathMatch(.*)*', name: 'not-found', redirect: '/' })

    return router;
}

