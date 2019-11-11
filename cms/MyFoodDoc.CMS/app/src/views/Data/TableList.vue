<template>
  <ColabDataTable 
    title="Ingredients" 
    viewModel="TableViewModel" 
    :headers="mainHeaders"
  >
    <template v-slot:item="{ item, edit }">
      <td>
        <InlineEdit
          rules="required|max:35"
          fieldtype="text"
          :value.sync="item.Name"
          :edit="edit"
        />
      </td>
      <td class="text-right">
        <InlineEdit
          rules="required"
          fieldtype="number"
          :value.sync="item.Amount"
          :edit="edit"
        />
      </td>
    </template>
  </ColabDataTable>
</template>

<script>
export default {
  components: {
    InlineEdit: () => import("@/components/helper/InlineEdit"),
    ColabDataTable: () => import("@/components/dotnetify/ColabDataTable")
  },
  
  data: () => ({
    mainHeaders: [      
      {
        sortable: true,
        value: "Name",
        text: "Name"
      },
      {
        sortable: true,
        text: "Amount",
        value: "Amount",
        align: "right"
      }
    ]
  })
};
</script>
