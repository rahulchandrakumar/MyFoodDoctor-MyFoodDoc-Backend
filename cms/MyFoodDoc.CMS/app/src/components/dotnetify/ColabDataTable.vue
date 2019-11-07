<template>
  <v-container fill-height fluid grid-list-xl>
    <v-layout justify-center wrap>
      <v-flex md12>
        <material-card
          color="green"
        >
        <v-row wrap pa-3 slot="header">
          <v-col 
            class="subheading font-weight-light mr-3 align-center"
          >
            <h4 
              class="title font-weight-light mb-2"
            >
              {{ title }}
            </h4>
          </v-col>
          <v-col cols="1">
            <v-btn
                v-on:click="onAdd"
                class="v-btn--simple ma-0"
                color="success"
                icon
                small
              >
                <v-icon 
                  color="white"
                  style="font-size:35px"
                >
                  mdi-plus-box
                </v-icon>
              </v-btn>
          </v-col>
        </v-row>

          <v-data-table :headers="mainHeaders" :items="Items" sort-by="Id" hide-default-footer>
            <template slot="item" slot-scope="{ item }">
              <tr>
                <td>{{ item.Id }}</td>
                <slot name="item" 
                    v-bind:item="item" 
                    v-bind:edit="item.LockDate != null && (now - item.LockDate <= editTime) && item.Editor == username"
                    >
                </slot>
                <td>
                  <v-btn
                    v-if="item.LockDate == null || (now - item.LockDate > editTime)"
                    v-on:click="onBeginEdit(item)"
                    class="v-btn--simple"
                    color="success"
                    icon
                  >
                    <v-icon color="primary">
                      mdi-pencil
                    </v-icon>
                  </v-btn>
                  <v-btn
                    v-else-if="item.Editor == username"
                    v-on:click="onSave(item)"
                    class="v-btn--simple"
                    color="success"
                    icon
                  >
                    <v-icon color="primary">
                      mdi-content-save
                    </v-icon>
                  </v-btn>
                  
                  <v-tooltip bottom v-else>
                      <template v-slot:activator="{ on }">
                        <v-btn v-on="on"
                          class="v-btn--simple"
                          color="success"
                          icon
                        >
                          <v-icon color="primary">
                            mdi-lock
                          </v-icon>
                        </v-btn>
                      </template>
                      <span>{{ item.Editor }}</span>
                    </v-tooltip>
                </td>
                <td>
                  <v-btn
                    v-if="item.LockDate == null || (now - item.LockDate > editTime)"
                    v-on:click="onRemove(item)"
                    class="v-btn--simple"
                    color="danger"
                    icon
                  >
                    <v-icon color="error">
                      mdi-close
                    </v-icon>
                  </v-btn>
                  <v-btn
                    v-else-if="item.Editor == username"
                    v-on:click="onCancel(item)"
                    class="v-btn--simple"
                    color="danger"
                    icon
                  >
                    <v-icon color="error">
                      mdi-undo
                    </v-icon>
                  </v-btn>
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

const editTime = 300000

export default {
  name: 'ColabDataTable',
  props: ['viewModel', 'headers', 'title'],
  created() {
    let token = this.$store.state.user.token
    let headers = { Authorization: "Bearer " + token } 
    this.vm = dotnetify.vue.connect(this.viewModel, this, { headers });

    this.username = this.$store.state.user.userInfo.username

    var self = this
    setInterval(() => {
         self.now = Date.now()
      }, 1000)
  },
  destroyed() {
    this.vm.$destroy();
  },
  data() {
      var mainHeaders = [{
                sortable: true,
                text: "Id",
                value: "Id"
            }]
            .concat(this.headers)
            .concat([      
            {
                sortable: false,
                width: "25px"
            },
            {
                sortable: false,
                width: "25px"
            }
            ]);

      return {
            mainHeaders: mainHeaders,
            now: Date.now(),
            username: '',
            Items: [],
            editTime: editTime
          }
},
  methods: {
    onAdd: function () {
      this.Items.unshift({
        LockDate: Date.now(),
        Editor: this.username
      })
    },
    onBeginEdit(item) {
      item.original = Object.assign({}, item)
      item.LockDate = Date.now()
      item.Editor = this.username
      this.vm.$dispatch({ Update: item });
    },
    onRemove(item) {
      if (item.LockDate == null || (this.now - item.LockDate > editTime))
        this.vm.$dispatch({ Remove: item.Id });
    },
    onSave(item) {
      item.original = null;
      item.LockDate = null;
      item.Editor = null;

      if (item.Id == null) {
        this.Items.shift();
        this.vm.$dispatch({ Add: item });
      }
      else
        this.vm.$dispatch({ Update: item });
    },
    onCancel(item) {
      if (item.LockDate != null && (this.now - item.LockDate <= editTime) && item.Editor == this.username) {
        if (item.Id == null)
          this.Items.shift();
        else {
          Object.assign(item, item.original)
          this.vm.$dispatch({ Update: item });
        }
      }
    }
  }
};
</script>
