/**
 * Vue Router
 *
 * @library
 *
 * https://router.vuejs.org/en/
 */

// Lib imports
import Vue from "vue";
//import VueAnalytics from "vue-analytics";
import Router from "vue-router";
import Meta from "vue-meta";

// Routes
import paths from "./paths";

import store from "@/store";

function route(path, view, name, meta) {
  return {
    name: name || view,
    path,
    component: resolve => import(`@/views/${view}.vue`).then(resolve),
    meta: meta
  };
}

Vue.use(Router);

// Create a new router
const router = new Router({
  mode: "history",
  routes: paths
    .map(path => route(path.path, path.view, path.name, path.meta))
    .concat([{ path: "*", redirect: "/" }]),
  scrollBehavior(to, from, savedPosition) {
    if (savedPosition) {
      return savedPosition;
    }
    if (to.hash) {
      return { selector: to.hash };
    }
    return { x: 0, y: 0 };
  }
});

router.beforeEach(async (to, from, next) => {
  if (to.path === "/logout") {
    await store.dispatch("user/logout");
  }
  if (to.meta.requiresAuth != false) {
    let loggedIn = store.state.user.userInfo.isAuthenticated;
    let roles = store.state.user.userInfo.roles;
    if (!loggedIn) {
      next("/login");
      return;
    }
    if (!roles.includes(to.meta.role)) {
      next("/");
      return;
    }
  }
  next();
});

Vue.use(Meta);

// Bootstrap Analytics
// Set in .env
// https://github.com/MatteoGabriele/vue-analytics
/*if (process.env.GOOGLE_ANALYTICS) {
  Vue.use(VueAnalytics, {
    id: process.env.GOOGLE_ANALYTICS,
    router,
    autoTracking: {
      page: process.env.NODE_ENV !== "development"
    }
  });
}*/

export default router;
