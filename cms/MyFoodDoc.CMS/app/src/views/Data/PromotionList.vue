<template>
  <ColabDataTable
    title="Promotions"
    editor-title-suffix="promotion"
    view-model="PromotionsViewModel"
    :headers="mainHeaders"
    :before-save="beforeSave"
  >
    <template v-slot:item.Disabled="{ item }">
      <v-checkbox
        :input-value="!item.Disabled"
        readonly
      />
    </template>
    <template v-slot:item.InsuranceId="{ item }">
      {{ translateInsurance(item.InsuranceId) }}
    </template>
    <template v-slot:item.Coupons="{ item }">
      {{ item.CouponCount }} ({{ item.CouponCount - item.UsedCouponCount }} / {{ item.UsedCouponCount }})
    </template>
    <template v-slot:item.StartDate="{ item }">
      {{ item.StartDate | moment(displayDateFormat) }}
    </template>
    <template v-slot:item.EndDate="{ item }">
      {{ item.EndDate | moment(displayDateFormat) }}
    </template>
    <template v-slot:item.DownloadCoupons="{ item }">
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
          v-model="item.Title"
          :label="mainHeaders.filter(h => h.value == 'Title')[0].text"
          rules="required|max:35"
          :counter="35"
        />
      </v-row>
      <v-row>
        <VeeSelect
          v-model="item.InsuranceId"
          :items="insuranceList"
          item-text="name"
          item-value="id"
          :label="mainHeaders.filter(h => h.value == 'InsuranceId')[0].text"
          rules="required"
        />
      </v-row>
      <v-row>
        <VeeDatePicker
          v-model="item.StartDate"
          :label="mainHeaders.filter(h => h.value == 'StartDate')[0].text"
          :rules="{ required: true, dateLess: { date: item.EndDate } }"
        />
      </v-row>
      <v-row>
        <VeeDatePicker
          v-model="item.EndDate"
          :label="mainHeaders.filter(h => h.value == 'EndDate')[0].text"
          :rules="{ required: true, dateMore: { date: item.StartDate } }"
        />
      </v-row>
      <v-row v-if="item.Id == null">
        <VeeFileInput
          v-model="item.File"
          label="CSV file"
          rules="required"
          accept=".csv,.txt"
        />
      </v-row>
      <v-row>
        <v-checkbox
          v-model="item.Disabled"
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
    ColabDataTable: () => import("@/components/dotnetify/ColabDataTable"),
    VeeTextField: () => import("@/components/inputs/VeeTextField"),
    VeeSelect: () => import("@/components/inputs/VeeSelect"),
    VeeDatePicker: () => import("@/components/inputs/VeeDatePicker"),
    VeeFileInput: () => import("@/components/inputs/VeeFileInput")
  },

  data: () => ({
    mainHeaders: [
      {
        sortable: true,
        value: "Title",
        text: "Title"
      },
      {
        sortable: true,
        value: "Disabled",
        text: "Active"
      },{
        sortable: true,
        value: "InsuranceId",
        text: "Insurance"
      },{
        sortable: true,
        value: "Coupons",
        text: "Coupons"
      },{
        sortable: true,
        value: "StartDate",
        text: "Start"
      },{
        sortable: true,
        value: "EndDate",
        text: "End"
      },
      {
        sortable: false,
        value: "DownloadCoupons"
      }
    ],
    insuranceList: [],
    displayDateFormat: displayDateFormat
  }),
  async mounted() {
    this.insuranceList = await this.$store.dispatch("dictionaries/getinsuranceList")
  },
  methods: {
    async beforeSave(item) {
      if (item.File != null)
        item.TempFileId = await integration.files.uploadTemp(item.File);
    },
    async downloadCoupons(item) {
      if (item.Id != null) {
        integration.files.downloadCoupons(item.Id);
      }
    },
    translateInsurance(value) {
      return value == null ? null : this.insuranceList.filter(v => v.id == value)[0].name
    },
  }
};
</script>
