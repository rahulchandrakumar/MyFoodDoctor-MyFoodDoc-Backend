<template>
  <v-container fill-height fluid grid-list-xl>
    <v-layout justify-center wrap>
      <v-flex md12>
        <material-chart-card
          :data="totalUsers"
          ratio="16:9"
          color="green"
          type="Line"
        >
          <h3 class="title font-weight-light">
            Total patients
          </h3>
        </material-chart-card>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
export default {
  computed: {
    totalUsers() {
      return this.makeChartData(this.$store.getters["dashboard/totalUsers"])
    },
    loaded() {
      return this.$store.getters["dashboard/loaded"]
    }
  },
  async mounted() {
    this.$store.dispatch("dashboard/loadItems")
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
