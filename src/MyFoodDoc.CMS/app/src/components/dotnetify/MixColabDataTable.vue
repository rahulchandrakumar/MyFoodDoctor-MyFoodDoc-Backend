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
                v-if="couldAdd || !readonly"
                :dialog.sync="dialog"
                :item="editItem"
                :title-suffix="editorTitleSuffix"
                :edit-time="editTime"
                :could-add="couldAdd"
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
            :items="tableItems"
            :search="search"
            sort-by="Id"
            item-key="Id"
            hide-default-footer
            disable-pagination
            filterable
            dense
            :show-expand="$scopedSlots['expanded-item'] != null"
            :loading="!IsLoaded && !tableLoaded"
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
                v-if="StateDict[item.id] == null || StateDict[item.id].LockDate == null || now - StateDict[item.id].LockDate > editTime"
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
                <span>{{ StateDict[item.id] == null ? '' : StateDict[item.id].Editor }}</span>
              </v-tooltip>
            </template>

            <template v-slot:item.removeAction="{ item }">
              <v-btn
                v-if="couldRemove && (StateDict[item.id] == null || StateDict[item.id].LockDate == null || now - StateDict[item.id].LockDate > editTime) && !item.Undeletable"
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
      <v-flex>
        <v-dialog
          v-model="errorDialog"
          max-width="290"
        >
          <v-card>
            <v-card-title class="headline">
              Error
            </v-card-title>
            <v-card-text>
              {{ errorText }}
            </v-card-text>
            <v-card-actions>
              <v-spacer />
              <v-btn
                color="green darken-1"
                text
                @click="errorDialog = false"
              >
                Ok
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
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
    storeName: {
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
    couldRemove: {
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
        value: "id"
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
      State: [],
      StateDict: {},
      IsLoaded: null,
      editTime: editTime,
      search: "",
      dialog: false,
      editItem: {},
      Error: null,
      errorDialog: false,
      errorText: null,

      Added: null,
      Updated: null,
      Deleted: null
    };
  },
  computed: {
    tableItems() {
      return this.$store.getters[this.storeName + "/items"]
    },
    tableLoaded() {
      return this.$store.getters[this.storeName + "/loaded"]
    }
  },
  watch: {
    State(newVal) {
      this.StateDict = {}
      if (newVal == null)
        return;
      for (var i=0;i<newVal.length;i++)
        this.StateDict[newVal[i].Id] = newVal[i]
    }
  },
  created() {
    let token = this.$store.state.user.token;
    let headers = { Authorization: "Bearer " + token };
    this.vm = dotnetify.vue.connect(this.viewModel, this, { headers });
    
    this.username = this.$store.state.user.userInfo.username;

    var self = this;

    this.$watch("Error", message => {
      self.errorText = message.ErrorMessage
      self.errorDialog = true
    })

    this.$watch("Added", async message => {
      await this.$store.dispatch(this.storeName + "/itemAdded", message)
    })
    this.$watch("Updated", async message => {
      await this.$store.dispatch(this.storeName + "/itemUpdated", message)
    })
    this.$watch("Deleted", async message => {
      await this.$store.dispatch(this.storeName + "/itemDeleted", message)
    })

    setInterval(() => {
      self.now = Date.now();
    }, 1000);
  },
  async mounted() {
    if (this.$scopedSlots["expanded-item"] != null)
      this.mainHeaders.push({ text: "", value: "data-table-expand" });

    this.vm.$dispatch({ Init: null });
    await this.$store.dispatch(this.storeName + "/loadItems")
  },
  destroyed() {
    this.vm.$destroy();
  },
  methods: {
    onBeginEdit(item) {
      this.editItem = Object.assign({}, item);

      var editprops = this.StateDict[item.id] || {};

      editprops.id = item.id;
      editprops.LockDate = Date.now();
      editprops.Editor = this.username;

      this.vm.$dispatch({ BeginEdit: editprops });
      this.dialog = true;
    },
    async onRemove(item) {
      if (item.LockDate == null || this.now - item.LockDate > editTime)
        if (await this.$confirm("Do you really want to delete this item?")) {
          await this.$store.dispatch(this.storeName + "/updateItem", { item: this.editItem })
          this.vm.$dispatch({ Remove: item.Id });
        }
    },
    async onSave(item) {
      if (this.beforeSave)
        await this.beforeSave(item);

      var editprops = this.StateDict[item.id];

      editprops.LockDate = null;
      editprops.Editor = null;

      this.vm.$dispatch({ CancelEdit: editprops.id });

      if (item.id == null) {
        await this.$store.dispatch(this.storeName + "/addItem", { item: this.editItem })
        this.vm.$dispatch({ Add: editprops });
      }
      else {
        await this.$store.dispatch(this.storeName + "/updateItem", { item: this.editItem })
        this.vm.$dispatch({ Update: editprops });
      }

      this.editItem = {};
    },
    onCancel() {
      if (this.editItem) {
        var editprops = this.StateDict[this.editItem.id]
        if (      
          editprops.LockDate != null &&
          this.now - editprops.LockDate <= editTime &&
          editprops.Editor == this.username
        ) {
          this.vm.$dispatch({ CancelEdit: this.editItem.id });
        }
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
.v-dialog .v-textarea {
  min-width: 65vw;
}
</style>
