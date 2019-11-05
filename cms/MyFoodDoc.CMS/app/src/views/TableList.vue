<template>
  <v-container fill-height fluid grid-list-xl>
    <v-layout justify-center wrap>
      <v-flex md12>
        <material-card
          color="green"
          title="Simple Table"
          text="Here is a subtitle for this table"
        >
          <v-data-table :headers="mainHeaders" :items="IngredientSizes" hide-default-footer>
            <template slot="item" slot-scope="{ item }">
              <tr>
                <td>{{ item.Id }}</td>
                <td>{{ item.Name }}</td>
                <td class="text-right">
                  {{ item.Amount }}
                </td>
              </tr>
            </template>
          </v-data-table>
        </material-card>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
import dotnetify from 'dotnetify/vue';

export default {
  created() {
    let token = this.$store.state.user.token
    let headers = { Authorization: "Bearer " + token } 
    this.vm = dotnetify.vue.connect("TableViewModel", this, { headers });
    this.dispatch = state => this.vm.$dispatch(state);
  },
  destroyed() {
    this.vm.$destroy();
  },
  data: () => ({
    mainHeaders: [
      {
        sortable: false,
        text: "Id",
        value: "Id"
      },
      {
        sortable: false,
        text: "Name",
        value: "Name"
      },
      {
        sortable: false,
        text: "Amount",
        value: "Amount",
        align: "right"
      },
    ],
    IngredientSizes: []
  })
};
</script>
