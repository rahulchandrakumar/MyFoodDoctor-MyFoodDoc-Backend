<template>
  <ColabDataTable
    title="Users"
    editor-title-suffix="user"
    store-name="users"
    :headers="mainHeaders"
  >
    <template v-slot:item.Role="{ item }">
      <span>
        {{ item.Role }}
      </span>
    </template>

    <template v-slot:editor="{ item }">
      <v-row>
        <VeeTextField
          v-model="item.username"
          :label="mainHeaders.filter(h => h.value == 'username')[0].text"
          :readonly="item.Id != null"
          rules="required|max:35"
          :counter="35"
        />
      </v-row>
      <v-row>
        <VeeTextField
          v-model="item.displayName"
          :label="mainHeaders.filter(h => h.value == 'displayName')[0].text"
          rules="required|max:35"
          :counter="35"
        />
      </v-row>
      <v-row>
        <VeeTextField
          v-model="item.password"
          :label="mainHeaders.filter(h => h.value == 'password')[0].text"
          :rules="(item.id == null ? 'required|' : '') + 'max:35'"
          :counter="35"
        />
      </v-row>
      <v-row>
        <VeeSelect
          v-model="item.role"
          :items="roles"
          :label="mainHeaders.filter(h => h.value == 'role')[0].text"
          rules="required"
        />
      </v-row>
    </template>
  </ColabDataTable>
</template>

<script>
import UserRoles from "@/enums/UserRoles";

export default {
  components: {
    ColabDataTable: () => import("@/components/signalR/ColabRDataTable"),
    VeeTextField: () => import("@/components/inputs/VeeTextField"),
    VeeSelect: () => import("@/components/inputs/VeeSelect")
  },
  data: () => ({
    mainHeaders: [
      {
        sortable: true,
        value: "username",
        text: "User Name"
      },
      {
        sortable: true,
        value: "displayName",
        text: "Display Name"
      },
      {
        sortable: false,
        value: "password",
        text: "Password"
      },
      {
        sortable: true,
        value: "role",
        text: "Role"
      }
    ],
    roles: []
  }),
  created() {
    this.roles = Object.values(UserRoles);
  }
};
</script>
