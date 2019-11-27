<template>
  <ColabDataTable
    title="Users"
    editor-title-suffix="user"
    view-model="UsersViewModel"
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
          v-model="item.Username"
          :label="mainHeaders.filter(h => h.value == 'Username')[0].text"
          :readonly="item.Id != null"
          rules="required|max:35"
          :counter="35"
        />
      </v-row>
      <v-row>
        <VeeTextField
          v-model="item.DisplayName"
          :label="mainHeaders.filter(h => h.value == 'DisplayName')[0].text"
          rules="required|max:35"
          :counter="35"
        />
      </v-row>
      <v-row>
        <VeeTextField
          v-model="item.Password"
          :label="mainHeaders.filter(h => h.value == 'Password')[0].text"
          :rules="(item.Id == null ? 'required|' : '') + 'max:35'"
          :counter="35"
        />
      </v-row>
      <v-row>
        <VeeSelect
          v-model="item.Role"
          :items="roles"
          :label="mainHeaders.filter(h => h.value == 'Role')[0].text"
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
    ColabDataTable: () => import("@/components/dotnetify/ColabDataTable"),
    VeeTextField: () => import("@/components/inputs/VeeTextField"),
    VeeSelect: () => import("@/components/inputs/VeeSelect")
  },
  data: () => ({
    mainHeaders: [
      {
        sortable: true,
        value: "Username",
        text: "User Name"
      },
      {
        sortable: true,
        value: "DisplayName",
        text: "Display Name"
      },
      {
        sortable: false,
        value: "Password",
        text: "Password"
      },
      {
        sortable: true,
        value: "Role",
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
