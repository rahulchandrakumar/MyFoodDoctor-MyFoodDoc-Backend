<template>
  <v-main>
    <div id="core-view">
      <v-fade-transition mode="out-in">
        <router-view :key="$route.fullPath" />
      </v-fade-transition>
    </div>
    <core-footer v-if="$route.name !== 'Maps'" />
  </v-main>
</template>

<script>
export default {
  metaInfo() {
    return {
      title: "MyFoodDoc"
    };
  }
};
</script>

<style>
#core-view {
  padding-bottom: 100px;
  height: 100%;
}
</style>
