<template>
    <v-dialog v-model="innerDialog"
              class="colab-edit"
              @click:outside="close">
        <template v-if="couldAdd" v-slot:activator="{ on }">
            <v-btn class="v-btn--simple ma-0" color="success" icon
                   small @click="add"
                   v-on="on">
                <v-icon color="white" style="font-size:35px">
                    mdi-plus-box
                </v-icon>
            </v-btn>
        </template>

        <ValidationObserver ref="observer"
                            style="display: contents;">
            <v-card>
                <v-card-title>
                    <span class="headline">
                        {{
            `${
              formItem && formItem.id != null ? "Edit" : "Add"
            } ${titleSuffix} ${
              formItem && formItem.id != null ? "(id:" + formItem.id + ")" : ""
            }`
                        }}
                    </span>
                </v-card-title>

                <v-card-text>
                    <v-container>
                        <slot :item="formItem" />
                    </v-container>
                </v-card-text>

                <v-card-actions>
                    <v-spacer />
                    <v-btn v-if="formItem.id && (!link.visible || link.visible(formItem))" v-for="link in childLinks" :key="link.path"
                           color="blue darken-1"
                           text
                           @click="navigateToChild(link.path)">
                        {{ link.title }}
                    </v-btn>
                    <v-btn color="blue darken-1"
                           text
                           @click="close">
                        Cancel
                    </v-btn>
                    <v-btn color="blue darken-1"
                           text
                           @click="checkSave">
                        Save
                    </v-btn>
                </v-card-actions>
            </v-card>
        </ValidationObserver>
    </v-dialog>
</template>

<script>
    export default {
        props: {
            dialog: {
                type: Boolean,
                required: true
            },
            titleSuffix: {
                type: String,
                required: true
            },
            editTime: {
                type: Number,
                required: true
            },
            item: {
                type: Object,
                default: null
            },
            beforeAdd: {
                type: Function,
                default: null
            },
            couldAdd: {
                type: Boolean,
                default: true
            },
            childLinks: {
                type: Array,
                default: null
            }
        },
        data() {
            return {
                formItem: this.item,
                timeout: null,
                innerDialog: this.dialog
            };
        },
        watch: {
            item(newval) {
                this.formItem = newval;
            },
            dialog(newval) {
                this.innerDialog = newval;
                if (newval) this.timeout = setInterval(this.close, this.editTime);
            },
            innerDialog(newval) {
                this.$emit("update:dialog", newval);
            }
        },
        methods: {
            async add() {
                this.formItem = null;
                this.formItem = {};

                if (this.beforeAdd)
                    await this.beforeAdd(this.formItem);

                this.$emit("update:dialog", true);
            },
            async checkSave() {
                if ((await this.$refs.observer.validate()) && !this.$refs.observer.invalid)
                    this.save();
            },
            save() {
                clearInterval(this.timeout);
                this.$emit("update:dialog", false);
                this.$emit("save", this.formItem);
            },
            close() {
                clearInterval(this.timeout);
                this.$emit("update:dialog", false);
                this.$emit("cancel");
            },
            async navigateToChild(path) {
                if ((await this.$refs.observer.validate()) && !this.$refs.observer.invalid) {
                    this.save();
                    this.$router.push({ name: path, params: { parentId: this.item.id } });
                }
            }
        }
    };
</script>

<style>
    .v-dialog {
        max-width: 80vw;
        width: unset !important;
        min-width: 350px;
    }
</style>
