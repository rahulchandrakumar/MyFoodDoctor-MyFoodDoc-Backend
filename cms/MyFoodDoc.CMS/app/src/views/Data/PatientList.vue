<template>
  <ColabDataTable 
    title="Patients" 
    viewModel="PatientsViewModel" 
    :headers="mainHeaders"
    :readonly="true"
  >
    <template v-slot:item.Sex="{ item }">
        {{ translateSex(item.Sex) }}
    </template>
    <template v-slot:item.Birth="{ item }">
      {{ item.Birth | moment("DD.MM.YYYY") }}
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
            <v-col>
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
        </v-container>
      </td>
    </template>
  </ColabDataTable>
</template>

<script>
import Sex from "@/enums/Sex"

export default {
  components: {
    ColabDataTable: () => import("@/components/dotnetify/ColabDataTable")
  },
  
  data: () => ({
    mainHeaders: [      
      {
        sortable: true,
        value: "FullName",
        text: "Name"
      },
      {
        sortable: false,
        value: "Email",
        text: "Email"
      },
      {
        sortable: true,
        value: "Insurance",
        text: "Insurance"
      },
      {
        filterable: false,
        sortable: true,
        value: "Sex",
        text: "Sex"
      },
      {
        sortable: true,
        value: "Height",
        text: "Height"
      },
      {
        filterable: false,
        sortable: true,
        text: "Birth",
        value: "Birth"
      }
    ]
  }),

  methods: {
    translateSex(value) {
      return value == Sex.MALE ? "Male" : "Female"
    },
    makeChartData(items) {
      return {
        labels: items.map(i => this.$moment(i.Created).format("DD MMM")),
        series: [{
          data: items.map(i => i.Value)
        }]
      }
    }
  }
};
</script>
