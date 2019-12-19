<template>
  <ColabDataTable
    title="Patients"
    store-name="patients"
    :headers="mainHeaders"
    :readonly="true"
  >
    <template v-slot:expanded-item="{ headers, item }">
      <td :colspan="headers.length">
        <v-container>
          <v-row>
            <v-col>
              <material-chart-card
                :data="makeChartData(item.weight)"
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
                :data="makeChartData(item.abdominalGirth)"
                ratio="16:9"
                color="green"
                type="Line"
              >
                <h4 class="title font-weight-light">
                  Abdominal girth
                </h4>
              </material-chart-card>
            </v-col>
          </v-row>
          <v-row v-if="item.motivations != null && item.motivations.length > 0">
            <h6 class="font-weight-light">
              Motivations: {{ item.motivations.join(',') }}
            </h6>
          </v-row>
          <v-row v-if="item.indications != null && item.indications.length > 0">
            <h6 class="font-weight-light">
              Indications: {{ item.indications.join(',') }}
            </h6>
          </v-row>
        </v-container>
      </td>
    </template>
  </ColabDataTable>
</template>

<script>
import Gender from "@/enums/Gender";

export default {
  components: {
    ColabDataTable: () => import("@/components/signalR/ColabRDataTable")
  },

  data() {
    return {
      mainHeaders: [{
          sortable: false,
          value: "email",
          text: "Email"
        }, {
          sortable: true,
          value: "insurance",
          text: "Insurance"
        }, {
          filterable: false,
          sortable: true,
          value: "gender",
          text: "Gender"
        }, {
          sortable: true,
          value: "height",
          text: "Height"
        }, {
          filterable: false,
          sortable: true,
          text: "Age",
          value: "age"
        }
      ],
      insuranceList: [],
    }
  },
  async created() {
    this.insuranceList = await this.$store.dispatch("dictionaries/getinsuranceList")
  },
  methods: {
    makeChartData(items) {
      return {
        labels: items.map(i => this.$moment(i.created).format("DD MMM")),
        series: [
          {
            data: items.map(i => i.value)
          }
        ]
      };
    }
  }
};
</script>
