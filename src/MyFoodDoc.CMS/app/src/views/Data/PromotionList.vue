<template>
  <ColabDataTable
    title="Promotions"
    editor-title-suffix="promotion"
    store-name="promotions"
    :headers="mainHeaders"
    :before-save="beforeSave"
  >
    <template v-slot:item.disabled="{ item }">
      <v-checkbox
        :input-value="!item.disabled"
        readonly
      />
    </template>
    <template v-slot:item.coupons="{ item }">
      {{ item.couponCount }} ({{ item.couponCount - item.usedCouponCount }} / {{ item.usedCouponCount }})
    </template>
    <template v-slot:item.startDate="{ item }">
      {{ item.startDate | moment(displayDateFormat) }}
    </template>
    <template v-slot:item.endDate="{ item }">
      {{ item.endDate | moment(displayDateFormat) }}
    </template>
    <template v-slot:item.downloadCoupons="{ item }">
      <v-btn
        class="v-btn--simple"
        color="success"
        icon
        @click="downloadCoupons(item)"
      >
        <v-icon color="success">
          mdi-download
        </v-icon>
      </v-btn>
    </template>

    <template v-slot:editor="{ item }">
      <v-row>
        <VeeTextField
          v-model="item.title"
          :label="mainHeaders.filter(h => h.value == 'title')[0].text"
          rules="required|max:35"
          :counter="35"
        />
      </v-row>
      <v-row>
        <VeeSelect
          v-model="item.insuranceId"
          :items="insuranceList"
          item-text="name"
          item-value="id"
          :label="mainHeaders.filter(h => h.value == 'insurance')[0].text"
          rules="required"
        />
      </v-row>
      <v-row>
        <VeeDatePicker
          v-model="item.startDate"
          :label="mainHeaders.filter(h => h.value == 'startDate')[0].text"
          :rules="{ required: true, dateLess: { date: item.endDate } }"
        />
      </v-row>
      <v-row>
        <VeeDatePicker
          v-model="item.endDate"
          :label="mainHeaders.filter(h => h.value == 'endDate')[0].text"
          :rules="{ required: true, dateMore: { date: item.startDate } }"
        />
      </v-row>
      <v-row v-if="item.id == null">
        <VeeFileInput
          v-model="item.file"
          label="CSV file"
          rules="required"
          accept=".csv,.txt"
        />
      </v-row>
      <v-row>
        <v-checkbox
          v-model="item.disabled"
          label="Disabled"
        />
      </v-row>
    </template>
  </ColabDataTable>
</template>

<script>
import integration from "@/integration";
import { displayDateFormat } from "@/utils/Consts.js"

export default {
  components: {
    ColabDataTable: () => import("@/components/signalR/ColabRDataTable"),
    VeeTextField: () => import("@/components/inputs/VeeTextField"),
    VeeSelect: () => import("@/components/inputs/VeeSelect"),
    VeeDatePicker: () => import("@/components/inputs/VeeDatePicker"),
    VeeFileInput: () => import("@/components/inputs/VeeFileInput")
  },

  data() {
    var self = this;

    return {
      mainHeaders: [
        {
          sortable: true,
          value: "title",
          text: "Title"
        },
        {
          sortable: true,
          value: "disabled",
          text: "Active"
        },{
          sortable: true,
          filterable: true,
          value: "insurance",
          text: "Insurance"
        },{
          sortable: true,
          value: "coupons",
          text: "Coupons"
        },{
          sortable: true,
          value: "startDate",
          text: "Start"
        },{
          sortable: true,
          value: "endDate",
          text: "End"
        },
        {
          sortable: false,
          value: "downloadCoupons"
        }
      ],
      insuranceList: [],
      displayDateFormat: displayDateFormat
    }
  },
  async mounted() {
    this.insuranceList = await this.$store.dispatch("dictionaries/getInsuranceList")
  },
  methods: {
    async beforeSave(item) {
      if (item.file != null)
        item.tempFileId = await integration.files.uploadTemp(item.file);
    },
    async downloadCoupons(item) {
      if (item.id != null) {
        integration.files.downloadCoupons(item.id);
      }
    }
  }
};
</script>
