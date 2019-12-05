<template>
  <ColabDataTable
    title="Patients"
    view-model="PatientsViewModel"
    :headers="mainHeaders"
    :readonly="true"
  >
    <template v-slot:item.InsuranceId="{ item }">
      {{ translateInsurance(item.InsuranceId) }}
    </template>
    <template v-slot:item.Gender="{ item }">
      {{ translateSex(item.Gender) }}
    </template>
    <template v-slot:item.Birth="{ item }">
      {{ item.Birth | moment(displayDateFormat) }}
    </template>

    <template v-slot:expanded-item="{ headers, item }">
      <td :colspan="headers.length">
        <v-container>
          <v-row>
            <v-col>
              <material-chart-card
                :data="makeChartData(item.Weight)"
                ratio="16:9"
                color="info"
                type="Line"
              >
                <h4 class="title font-weight-light">
                  Weight
                </h4>
              </material-chart-card>
            </v-col>
            <v-col>
              <material-chart-card
                :data="makeChartData(item.AbdominalGirth)"
                ratio="16:9"
                color="green"
                type="Line"
              >
                <h4 class="title font-weight-light">
                  Abdominal girth
                </h4>
              </material-chart-card>
            </v-col>
            <v-col v-if="false">
              <material-chart-card
                :data="makeChartData(item.BloodSugar)"
                ratio="16:9"
                color="red"
                type="Line"
              >
                <h4 class="title font-weight-light">
                  Blood sugar
                </h4>
              </material-chart-card>
            </v-col>
          </v-row>
          <v-row v-if="item.Motivation != null">
            <h6 class="font-weight-light">
              Motivations: {{ item.Motivation.join(',') }}
            </h6>
          </v-row>
        </v-container>
      </td>
    </template>
  </ColabDataTable>
</template>

<script>
import Gender from "@/enums/Gender";
import { displayDateFormat } from "@/utils/Consts.js"

export default {
  components: {
    ColabDataTable: () => import("@/components/dotnetify/ColabDataTable")
  },

  data() {
    return {
      mainHeaders: [{
          sortable: true,
          value: "FullName",
          text: "Name"
        }, {
          sortable: false,
          value: "Email",
          text: "Email"
        }, {
          sortable: true,
          value: "InsuranceId",
          text: "Insurance"
        }, {
          filterable: false,
          sortable: true,
          value: "Gender",
          text: "Gender"
        }, {
          sortable: true,
          value: "Height",
          text: "Height"
        }, {
          filterable: false,
          sortable: true,
          text: "Birth",
          value: "Birth"
        }
      ],
      insuranceList: [],
      displayDateFormat: displayDateFormat
    }
  },
  async mounted() {
    this.insuranceList = await this.$store.dispatch("dictionaries/getinsuranceList")
  },
  methods: {
    translateSex(value) {
      return value == null ? null : value == Gender.MALE ? "Male" : "Female";
    },
    translateInsurance(value) {
      return value == null ? null : this.insuranceList.filter(v => v.id == value)[0].name
    },
    makeChartData(items) {
      return {
        labels: items.map(i => this.$moment(i.Created).format("DD MMM")),
        series: [
          {
            data: items.map(i => i.Value)
          }
        ]
      };
    }
  }
};
</script>
