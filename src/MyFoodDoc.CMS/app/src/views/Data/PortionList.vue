<template>
  <ColabDataTable
    title="Portions"
    editor-title-suffix="portion"
    store-name="portions"
    :headers="mainHeaders"
    :could-add="false"
    :could-remove="false"
  >
    <template v-slot:filter="{ filter, loading }">
        <!--
      <v-radio-group 
        v-model="filter.state"
        :disabled="loading"
        row 
        hide-details
      >
        <v-radio label="Have to specify" value="0" />
        <v-radio label="Not specified" value="1" />
        <v-radio label="Specified" value="2" />
      </v-radio-group>
            -->
    </template>
    <template v-slot:editor="{ item }">
        <v-row>
            <VeeTextField v-model="item.foodId"
                          :label="mainHeaders.filter(h => h.value == 'foodId')[0].text"
                          readonly />
        </v-row>
        <v-row>
            <VeeTextField v-model="item.foodName"
                          :label="mainHeaders.filter(h => h.value == 'foodName')[0].text"
                          readonly />
        </v-row>
        <v-row>
            <VeeTextField v-model="item.servingId"
                          :label="mainHeaders.filter(h => h.value == 'servingDescription')[0].text"
                          readonly />
        </v-row>
        <v-row>
            <VeeTextField v-model="item.servingDescription"
                          :label="mainHeaders.filter(h => h.value == 'servingDescription')[0].text"
                          readonly />
        </v-row>
        <v-row>
            <VeeTextField v-model="item.calories"
                          label="Calories"
                          rules="decimal"
                          number />
        </v-row>
        <v-row>
            <VeeTextField v-model="item.carbohydrate"
                          label="Carbohydrate"
                          rules="decimal"
                          number />
        </v-row>
        <v-row>
            <VeeTextField v-model="item.protein"
                          label="Protein"
                          rules="decimal"
                          number />
        </v-row>
        <v-row>
            <VeeTextField v-model="item.fat"
                          label="Fat"
                          rules="decimal"
                          number />
        </v-row>
        <v-row>
            <VeeTextField v-model="item.saturatedFat"
                          label="Saturated fat"
                          rules="decimal"
                          number />
        </v-row>
        <v-row>
            <VeeTextField v-model="item.polyunsaturatedFat"
                          label="Polyunsaturated fat"
                          rules="decimal"
                          number />
        </v-row>
        <v-row>
            <VeeTextField v-model="item.monounsaturatedFat"
                          label="Monounsaturated fat"
                          rules="decimal"
                          number />
        </v-row>
        <v-row>
            <VeeTextField v-model="item.cholesterol"
                          label="Cholesterol"
                          rules="decimal"
                          number />
        </v-row>
        <v-row>
            <VeeTextField v-model="item.sodium"
                          label="Sodium"
                          rules="decimal"
                          number />
        </v-row>
        <v-row>
            <VeeTextField v-model="item.potassium"
                          label="Potassium"
                          rules="decimal"
                          number />
        </v-row>
        <v-row>
            <VeeTextField v-model="item.fiber"
                          label="Fiber"
                          rules="decimal"
                          number />
        </v-row>
        <v-row>
            <VeeTextField v-model="item.sugar"
                          label="Sugar"
                          rules="decimal"
                          number />
        </v-row>
    </template>
  </ColabDataTable>
</template>

<script>
export default {
  components: {
    ColabDataTable: () => import("@/components/signalR/ColabRDataTable"),
    VeeTextField: () => import("@/components/inputs/VeeTextField")
  },

  data: () => ({
      mainHeaders: [
      {
        sortable: true,
        value: "id",
        text: "Id"
      },
      {
        sortable: true,
        value: "foodId",
        text: "Food Id"
      },
      {
        sortable: true,
        value: "foodName",
        text: "Food name"
      },
      {
        sortable: true,
        value: "servingId",
        text: "Serving Id"
      },
      {
        sortable: true,
        text: "Serving description",
        value: "servingDescription"
      },
      {
        sortable: true,
        text: "Last synchronized",
        value: "lastSynchronized",
        align: "right"
      }
    ]
  }),
};
</script>
