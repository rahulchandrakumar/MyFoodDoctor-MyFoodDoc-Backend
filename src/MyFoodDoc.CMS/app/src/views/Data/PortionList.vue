<template>
  <ColabDataTable
    title="Portions"
    editor-title-suffix="portion"
    store-name="portions"
    :headers="mainHeaders"
    :could-add="false"
    :could-remove="false"
  >
    <template v-slot:filter="{ filter }">
      <v-radio-group v-model="filter.state" row hide-details>
        <v-radio label="Have to specify" value="0" />
        <v-radio label="Not specified" value="1" />
        <v-radio label="Specified" value="2" />
      </v-radio-group>
    </template>
    <template v-slot:editor="{ item }">
      <v-row>
        <VeeTextField
          v-model="item.blsId"
          :label="mainHeaders.filter(h => h.value == 'blsId')[0].text"
          readonly
        />
      </v-row>
      <v-row>
        <VeeTextField
          v-model="item.name"
          :label="mainHeaders.filter(h => h.value == 'name')[0].text"
          readonly
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
    ColabDataTable: () => import("@/components/signalR/ColabRDataTable"),
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
