<template>
  <ColabDataTable
    title="Ingredients"
    editor-title-suffix="ingredient"
    view-model="TableViewModel"
    :headers="mainHeaders"
  >
    <template v-slot:editor="{ item }">
      <v-row>
        <VeeTextField
          v-model="item.Name"
          :label="mainHeaders.filter(h => h.value == 'Name')[0].text"
          rules="required|max:35"
          :counter="35"
        />
      </v-row>
      <v-row>
        <VeeTextField
          v-model="item.Amount"
          :label="mainHeaders.filter(h => h.value == 'Amount')[0].text"
          rules="required|decimal"
        />
      </v-row>
    </template>
  </ColabDataTable>
</template>

<script>
export default {
  components: {
    ColabDataTable: () => import("@/components/dotnetify/ColabDataTable"),
    VeeTextField: () => import("@/components/inputs/VeeTextField")
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
