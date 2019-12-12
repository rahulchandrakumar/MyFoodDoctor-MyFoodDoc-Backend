<template>
  <ColabDataTable
    title="Patients"
    store-name="patients"
    :headers="mainHeaders"
    :readonly="true"
  >
    <template v-slot:item.birth="{ item }">
      {{ item.birth | moment(displayDateFormat) }}
    </template>

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
          <v-row v-if="item.motivation != null">
            <h6 class="font-weight-light">
              Motivations: {{ item.motivation.join(',') }}
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
    ColabDataTable: () => import("@/components/dotnetify/ColabRDataTable")
  },

  data() {
    return {
      mainHeaders: [{
          sortable: true,
          value: "fullName",
          text: "Name"
        }, {
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
          text: "Birth",
          value: "birth"
        }
      ],
      insuranceList: [],
      displayDateFormat: displayDateFormat
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
