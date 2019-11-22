<template>
  <v-app v-if="$route.matched.some(({ path }) => path === '/login')">
    <core-view />
  </v-app>
  <v-app v-else>
    <!--<core-filter />-->
    <core-toolbar />
    <core-drawer />
    <core-view />
  </v-app>
</template>

<style lang="scss">
@import "@/styles/index.scss";

/* Remove in 1.2 */
.v-datatable thead th.column.sortable i {
  vertical-align: unset;
}
</style>
