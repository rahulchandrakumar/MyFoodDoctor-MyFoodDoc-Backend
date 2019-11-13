<template>
  <ColabDataTable 
    title="Ingredients"
    editorTitleSuffix="ingredient"
    viewModel="TableViewModel" 
    :headers="mainHeaders"
  >
    <template v-slot:editor="{ item }">
      <v-row>
        <VeeTextField
          v-model="item.Name" 
          :label="mainHeaders.filter(h => h.value == 'Name')[0].text"
          rules="required|max:35" :counter="35"
        ></VeeTextField>
      </v-row>
      <v-row>
        <VeeTextField 
          v-model="item.Amount" 
          :label="mainHeaders.filter(h => h.value == 'Amount')[0].text"
          rules="required|decimal"
        ></VeeTextField>
      </v-row>
    </template>
  </ColabDataTable>
</template>

<script>
export default {
  components: {
    ColabDataTable: () => import("@/components/dotnetify/ColabDataTable"),
    VeeTextField: () => import("@/components/inputs/VeeTextField"),
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
