<template>
  <ColabDataTable 
    title="Users" 
    viewModel="UsersViewModel" 
    :headers="mainHeaders"
  >
    <template v-slot:item="{ item, edit }">
      <td>
        <InlineEdit
          rules="required|max:35"
          fieldtype="text"
          :value.sync="item.Username"
          :edit="edit"
        />
      </td>
      <td>
        <InlineEdit
          rules="required|max:35"
          fieldtype="text"
          :value.sync="item.DisplayName"
          :edit="edit"
        />
      <td>
        <InlineEdit
          rules="required|max:35"
          fieldtype="text"
          :value.sync="item.Password"
          :edit="edit"
        />
      </td>
      <td>
        <InlineSelect
          :value.sync="item.Role"
          :edit="edit"
          :items="roles"
        />
      </td>
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
