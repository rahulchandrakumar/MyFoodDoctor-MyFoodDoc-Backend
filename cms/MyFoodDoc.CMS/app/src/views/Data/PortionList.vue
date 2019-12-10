<template>
  <ColabDataTable
    title="Portions"
    editor-title-suffix="portion"
    store-name="portions"
    :headers="mainHeaders"
    :could-add="false"
    :could-remove="false"
  >
    <template v-slot:editor="{ item }">
      <v-row>
        <VeeTextField
          v-model="item.name"
          :label="mainHeaders.filter(h => h.value == 'name')[0].text"
          rules="required|max:35"
          :counter="35"
        />
      </v-row>
      <v-row>
        <VeeTextField
          v-model="item.amount"
          :label="mainHeaders.filter(h => h.value == 'amount')[0].text"
          rules="required|decimal"
          number
        />
      </v-row>
    </template>
  </ColabDataTable>
</template>

<script>
export default {
  components: {
    ColabDataTable: () => import("@/components/dotnetify/ColabRDataTable"),
    VeeTextField: () => import("@/components/inputs/VeeTextField")
  },

  data: () => ({
    mainHeaders: [
      {
        sortable: true,
        value: "blsId",
        text: "BLS Id"
      },
      {
        sortable: true,
        value: "name",
        text: "Name"
      },
      {
        sortable: true,
        text: "Amount",
        value: "amount",
        align: "right"
      }
    ]
  }),
};
</script>
