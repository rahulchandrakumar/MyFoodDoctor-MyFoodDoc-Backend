<template>
  <v-container fill-height fluid grid-list-xl>
    <v-layout justify-center wrap>
      <v-flex md12>
        <material-card color="green">
          <v-row slot="header" wrap pa-3>
            <v-col class="subheading font-weight-light mr-3 align-center">
              <h4 class="title font-weight-light mb-2">
                {{ title }}
              </h4>
              <v-text-field
                v-model="search"
                class="mr-4"
                label="Search..."
                hide-details
              />
            </v-col>
            <v-col cols="1">
              <ColabEdit
                v-if="couldAdd && !readonly"
                :dialog.sync="dialog"
                :item="editItem"
                :title-suffix="editorTitleSuffix"
                :edit-time="editTime"
                @cancel="onCancel"
                @save="onSave"
              >
                <template slot-scope="scope">
                  <slot
                    name="editor" v-bind="scope"
                  />
                </template>
              </ColabEdit>
            </v-col>
          </v-row>

          <v-data-table
            :headers="mainHeaders"
            :items="Items"
            :search="search"
            sort-by="Id"
            item-key="Id"
            hide-default-footer
            disable-pagination
            filterable
            dense
            :show-expand="$scopedSlots['expanded-item'] != null"
          >
            <template
              v-for="slot in Object.keys($scopedSlots)"
              :slot="slot"
              slot-scope="scope"
            >
              <slot
                :name="slot" v-bind="scope"
              />
            </template>

            <template v-slot:item.editAction="{ item }">
              <v-btn
                v-if="item.LockDate == null || now - item.LockDate > editTime"
                class="v-btn--simple"
                color="success"
                icon
                @click="onBeginEdit(item)"
              >
                <v-icon color="primary">
                  mdi-pencil
                </v-icon>
              </v-btn>
              <v-tooltip v-else bottom>
                <template v-slot:activator="{ on }">
                  <v-btn
                    class="v-btn--simple" color="success" icon
                    v-on="on"
                  >
                    <v-icon color="primary">
                      mdi-lock
                    </v-icon>
                  </v-btn>
                </template>
                <span>{{ item.Editor }}</span>
              </v-tooltip>
            </template>

            <template v-slot:item.removeAction="{ item }">
              <v-btn
                v-if="item.LockDate == null || now - item.LockDate > editTime"
                class="v-btn--simple"
                color="danger"
                icon
                @click="onRemove(item)"
              >
                <v-icon color="error">
                  mdi-close
                </v-icon>
              </v-btn>
            </template>
          </v-data-table>
        </material-card>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
import dotnetify from "dotnetify/vue";
import ColabEdit from "./ColabEdit";

const editTime = 300000;

export default {
  name: "ColabDataTable",
  components: {
    ColabEdit: ColabEdit
  },
  props: {
    viewModel: {
      type: String,
      required: true
    },
    headers: {
      type: Array,
      required: true
    },
    title: {
      type: String,
      required: true
    },
    editorTitleSuffix: {
      type: String,
      default: ""
    },
    readonly: {
      type: Boolean,
      default: false
    },
    couldAdd: {
      type: Boolean,
      default: true
    },
    beforeSave: {
      type: Function,
      default: null
    }
  },
  data() {
    var mainHeaders = [
      {
        sortable: true,
        text: "Id",
        value: "Id"
      }
    ].concat(this.headers);

    if (!this.readonly)
      mainHeaders = mainHeaders.concat([
        {
          value: "editAction",
          sortable: false,
          width: "25px"
        },
        {
          value: "removeAction",
          sortable: false,
          width: "25px"
        }
      ]);

    return {
      mainHeaders: mainHeaders,
      now: Date.now(),
      username: "",
      Items: [],
      editTime: editTime,
      search: "",
      dialog: false,
      editItem: {}
    };
  },
  created() {
    let token = this.$store.state.user.token;
    let headers = { Authorization: "Bearer " + token };
    this.vm = dotnetify.vue.connect(this.viewModel, this, { headers });

    this.username = this.$store.state.user.userInfo.username;

    var self = this;
    setInterval(() => {
      self.now = Date.now();
    }, 1000);
  },
  mounted() {
    if (this.$scopedSlots["expanded-item"] != null)
      this.mainHeaders.push({ text: "", value: "data-table-expand" });
  },
  destroyed() {
    this.vm.$destroy();
  },
  methods: {
    onBeginEdit(item) {
      item.Original = Object.assign({}, item);
      item.LockDate = Date.now();
      item.Editor = this.username;
      this.vm.$dispatch({ Update: item });

      this.editItem = item;
      this.dialog = true;
    },
    onRemove(item) {
      if (item.LockDate == null || this.now - item.LockDate > editTime)
        this.vm.$dispatch({ Remove: item.Id });
    },
    async onSave(item) {
      if (this.beforeSave)
        await this.beforeSave(item);

      item.Original = null;
      item.LockDate = null;
      item.Editor = null;

      if (item.Id == null) this.vm.$dispatch({ Add: item });
      else this.vm.$dispatch({ Update: item });

      this.editItem = {};
    },
    onCancel() {
      if (
        this.editItem &&
        this.editItem.LockDate != null &&
        this.now - this.editItem.LockDate <= editTime &&
        this.editItem.Editor == this.username
      ) {
        Object.assign(this.editItem, this.editItem.Original);
        this.editItem.Original = null;
        this.vm.$dispatch({ Update: this.editItem });
      }
      this.editItem = {};
    }
  }
};
</script>

<style>
.v-card--material__header .v-text-field__slot .v-label {
  color: unset !important;
}
</style>
