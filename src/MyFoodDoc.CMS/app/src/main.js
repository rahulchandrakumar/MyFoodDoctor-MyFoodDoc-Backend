// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from "vue";

// Components
import "./components";

// Plugins
import "./plugins";
import vuetify from "./plugins/vuetify";
import { sync } from "vuex-router-sync";

// Application imports
import App from "./App";
import router from "@/router";
import store from "@/store";

// Sync store with router
sync(store, router);

Vue.config.productionTip = false;

/* eslint-disable no-new */
new Vue({
  vuetify,
  router,
  store,
  render: h => h(App)
}).$mount("#app");
