<template>
  <v-navigation-drawer
    id="app-drawer"
    v-model="inputValue"
    app
    dark
    floating
    persistent
    mobile-break-point="991"
    width="260"
  >
    <v-img :src="image" height="100%">
      <v-layout class="fill-height" tag="v-list" column>
        <v-list-item
          active-class="success"
          class="v-list-item"
          to="/user-profile">
          <v-list-item-avatar>
            <v-icon>mdi-account-circle</v-icon>
          </v-list-item-avatar>
          <v-list-item-title class="title">
            {{ username }}
          </v-list-item-title>
        </v-list-item>
        <v-divider />
        <v-list-item
          v-for="(link, i) in links"
          :key="i"
          :to="link.to"
          :active-class="color"
          class="v-list-item"
        >
          <v-list-item-action>
            <v-icon>{{ link.icon }}</v-icon>
          </v-list-item-action>
          <v-list-item-title v-text="link.text" />
        </v-list-item>
        <v-list-item
          active-class="success"
          class="v-list-item v-list-item--buy"
          to="/logout"
        >
          <v-list-item-action>
            <v-icon>mdi-package-up</v-icon>
          </v-list-item-action>
          <v-list-item-title class="font-weight-light">
            Logout
          </v-list-item-title>
        </v-list-item>
      </v-layout>
    </v-img>
  </v-navigation-drawer>
</template>

<script>
// Utilities
import { mapMutations, mapState } from "vuex";
import UserRoles from "@/enums/UserRoles"

export default {
  props: {
    opened: {
      type: Boolean,
      default: false
    }
  },
  created() {
    this.username = this.$store.state.user.userInfo.displayname

    let userRoles = this.$store.state.user.userInfo.roles
    let links = []
    if (userRoles.includes(UserRoles.VIEWER))
      links.push({
          to: "/",
          icon: "mdi-view-dashboard",
          text: "Dashboard"
      })
    
    if (userRoles.includes(UserRoles.EDITOR))
      links.push({
          to: "/table-list",
          icon: "mdi-clipboard-outline",
          text: "Table List"
        })

    if (userRoles.includes(UserRoles.ADMIN)) {
      links.push({
          to: "/users",
          icon: "mdi-account-multiple",
          text: "Users"
        },
        {
          to: "/typography",
          icon: "mdi-format-font",
          text: "Typography"
        },
        {
          to: "/icons",
          icon: "mdi-chart-bubble",
          text: "Icons"
        },
        {
          to: "/maps",
          icon: "mdi-map-marker",
          text: "Maps"
        },
        {
          to: "/notifications",
          icon: "mdi-bell",
          text: "Notifications"
        })
    }

    this.links = links
  },
  data: () => ({
    logo: "favicon.ico",
    links: [],
    username: ''
  }),
  computed: {
    ...mapState("app", ["image", "color"]),
    inputValue: {
      get() {
        return this.$store.state.app.drawer;
      },
      set(val) {
        this.setDrawer(val);
      }
    },
    items() {
      return this.$t("Layout.View.items");
    }
  },

  methods: {
    ...mapMutations("app", ["setDrawer", "toggleDrawer"])
  }
};
</script>

<style lang="scss">
#app-drawer {
  .v-list-item {
    border-radius: 4px;

    &--buy {
      margin-top: auto;
      margin-bottom: 17px;
    }
  }
}
</style>
