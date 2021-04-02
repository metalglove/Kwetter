import { createRouter, createWebHashHistory, Router, RouteRecordRaw } from 'vue-router';

export type KwetterRoute = {
    name: string,
    props?: Record<string, any>
}

export function createKwetterRouter(routes: KwetterRoute[]): Router {
    /* tslint:disable */
    const loadView = (name: string): any => import(`../views/${name}.vue`);
    /* tslint:enable */

    const router: Router = createRouter({
        history: createWebHashHistory(),
        routes: []
    });

    // Check whether the user is logged in.
    //router.beforeEach((to, from, next) => {
    //    const publicViews = ['/login', '/register'];
    //    const authRequired = !publicViews.includes(to.path);
    //    const loggedIn = localStorage.getItem('user');
    //    // If not, redirect the user to the login view.
    //    if (authRequired && !loggedIn) {
    //        return next('/login');
    //    }
    //    return next();
    //});

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

