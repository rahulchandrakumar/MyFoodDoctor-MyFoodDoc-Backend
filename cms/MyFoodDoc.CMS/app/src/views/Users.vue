<template>
  <ColabDataTable 
    title="Users" 
    viewModel="UsersViewModel" 
    :headers="mainHeaders"
  >
    <template v-slot:item.Username="{ item, edit }">
      <InlineEdit
        rules="required|max:35"
        fieldtype="text"
        :value.sync="item.Username"
        :edit="edit"
      />
    </template>
    <template v-slot:item.DisplayName="{ item, edit }">
      <InlineEdit
        rules="required|max:35"
        fieldtype="text"
        :value.sync="item.DisplayName"
        :edit="edit"
      />
    </template>
    <template v-slot:item.Password="{ item, edit }">
      <InlineEdit
        rules="required|max:35"
        fieldtype="text"
        :value.sync="item.Password"
        :edit="edit"
      />
    </template>
    <template v-slot:item.Role="{ item, edit }">
      <InlineSelect
        :value.sync="item.Role"
        :edit="edit"
        :items="roles"
      />
    </template>
  </ColabDataTable>
</template>

<script>
import UserRoles from "@/enums/UserRoles"

export default {
  components: {
    InlineEdit: () => import("@/components/helper/InlineEdit"),
    InlineSelect: () => import("@/components/helper/InlineSelect"),
    ColabDataTable: () => import("@/components/dotnetify/ColabDataTable")
  },
  created() {
      this.roles = Object.values(UserRoles)
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
  })
};
</script>
