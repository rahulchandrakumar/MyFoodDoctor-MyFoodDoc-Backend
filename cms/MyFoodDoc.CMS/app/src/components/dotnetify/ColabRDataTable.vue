<template>
  <v-container fill-height fluid grid-list-xl>
    <v-layout justify-center wrap>
      <v-flex md12>
        <material-card color="green">
          <v-row slot="header" wrap>
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
              <v-row v-if="filter" class="mt-1 ml-1" wrap>
                <slot
                  name="filter" :filter="filter"
                />
              </v-row>
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
            :server-items-length="tableItemsCount"
            :page.sync="page"
            disable-sort
            filterable
            dense
            :show-expand="$scopedSlots['expanded-item'] != null"
            :loading="!tableLoaded"
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
                v-if="stateDict[item.id] == null || stateDict[item.id].LockDate == null || now - stateDict[item.id].LockDate > editTime"
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
                <span>{{ stateDict[item.id] == null ? '' : stateDict[item.id].Editor }}</span>
              </v-tooltip>
            </template>

            <template v-slot:item.removeAction="{ item }">
              <v-btn
                v-if="couldRemove && (stateDict[item.id] == null || stateDict[item.id].LockDate == null || now - stateDict[item.id].LockDate > editTime) && !item.Undeletable"
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
import debounce from "debounce"

const editTime = 300000;

export default {
  name: "ColabDataTable",
  components: {
    ColabEdit: ColabEdit
  },
  props: {
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
      page: 1,
      now: Date.now(),
      username: "",
      editTime: editTime,
      search: "",
      dialog: false,
      editItem: {},
      Error: null,
      errorDialog: false,
      errorText: null,
    };
  },
  computed: {
    tableItems() {
      return this.$store.getters[this.storeName + "/items"]
    },
    tableItemsCount() {
      return this.$store.getters[this.storeName + "/total"]
    },
    tableLoaded() {
      return this.$store.getters[this.storeName + "/loaded"]
    },
    filter() {
      return this.$store.getters[this.storeName + "/filter"]
    },
    stateDict() {
      return this.$store.getters["edit-state/states"][this.storeName] || {}
    },
    addedItems() {
      return this.$store.getters["edit-state/addedItems"][this.storeName] || []
    },
    updatedItems() {
      return this.$store.getters["edit-state/updatedItems"][this.storeName] || []
    },
    deletedItems() {
      return this.$store.getters["edit-state/deletedItems"][this.storeName] || []
    }
  },
  watch: {
    async page(newVal) {
      await this.loadItems({ page: newVal, search: this.search, filter: this.filter })
    },
    async search(newVal) {
      if (this.debouncedSearch == null)
        this.debouncedSearch = debounce(async (page, search) => { await this.loadItems({ page, search }) }, 350)
      this.debouncedSearch(this.page, newVal)
    },
    filter: {
      deep: true,
      async handler(newVal) {
        await this.loadItems({ page: this.page, search: this.search, filter: newVal })
      }
    },
    async addedItems(newVal) {
      while((newVal || []).length > 0) {
        await this.$store.dispatch(this.storeName + "/itemAdded", { Id: newVal.pop() })
      }
    },
    async updatedItems(newVal) {
      while((newVal || []).length > 0) {
        await this.$store.dispatch(this.storeName + "/itemUpdated", { Id: newVal.pop() })
      }
    },
    async deletedItems(newVal) {
      while((newVal || []).length > 0) {
        await this.$store.dispatch(this.storeName + "/itemDeleted", { Id: newVal.pop() })
      }
    }
  },
  created() {
    this.username = this.$store.state.user.userInfo.username;

    var self = this;
    setInterval(() => {
      self.now = Date.now();
    }, 1000);
  },
  async mounted() {
    if (this.$scopedSlots["expanded-item"] != null)
      this.mainHeaders.push({ text: "", value: "data-table-expand" });

    await this.loadItems({ page: this.page, search: this.search, filter: this.filter })
    await this.$store.dispatch('edit-state/init', { groupName: this.storeName })
  },
  async destroyed() {
    await this.$store.dispatch('edit-state/removeGroup', { groupName: this.storeName })
  },
  methods: {
    async onBeginEdit(item) {
      this.editItem = Object.assign({}, item);

      var editprops = this.stateDict[item.id] || {};

      editprops.Id = item.id;
      editprops.LockDate = Date.now();
      editprops.Editor = this.username;

      await this.$store.dispatch('edit-state/addEntry', { groupName: this.storeName, entry: editprops })
      this.dialog = true;
    },
    async onRemove(item) {
      var editprops = this.stateDict[item.id];
      if (editprops != null && editprops.LockDate == null || this.now - editprops.LockDate > editTime)
        if (await this.$confirm("Do you really want to delete this item?")) {
          await this.$store.dispatch(this.storeName + "/updateItem", { item: this.editItem })
          await this.$store.dispatch('edit-state/removeEntry', { groupName: this.storeName, id: item.id })
        }
    },
    async onSave(item) {
      if (this.beforeSave)
        await this.beforeSave(item);

      var editprops = this.stateDict[item.id];

      editprops.LockDate = null;
      editprops.Editor = null;

      await this.$store.dispatch('edit-state/removeEntry', { groupName: this.storeName, id: editprops.Id })

      if (item.id == null) {
        await this.$store.dispatch(this.storeName + "/addItem", { item: this.editItem })
      }
      else {
        await this.$store.dispatch(this.storeName + "/updateItem", { item: this.editItem })
      }

      this.editItem = {};
    },
    async onCancel() {
      if (this.editItem) {
        var editprops = this.stateDict[this.editItem.id]
        if (      
          editprops.LockDate != null &&
          this.now - editprops.LockDate <= editTime &&
          editprops.Editor == this.username
        ) {
          await this.$store.dispatch('edit-state/removeEntry', { groupName: this.storeName, id: editprops.Id })
        }
      }
      this.editItem = {};
    },
    async loadItems({ page, search, filter }) {
      await this.$store.dispatch(this.storeName + "/loadItems", { page: this.page, search: this.search, filter: this.filter })
    }
  }
};
</script>

<style lang="scss">
.v-card--material__header .v-text-field__slot .v-label {
  color: unset !important;
}
.v-dialog .v-textarea {
  min-width: 65vw;
}
.v-data-table .v-data-footer {
  .v-data-footer__select {
    display: none;
  }
  .v-btn {
    color:rgba(0,0,0,.54) !important;
  }
}
</style>