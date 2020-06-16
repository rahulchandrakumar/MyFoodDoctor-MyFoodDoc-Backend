<template>
    <ColabDataTable title="Methods"
                    store-name="methods"
                    editor-title-suffix="method item"
                    :headers="mainHeaders"
                    :before-add="init"
                    :before-edit="beforeEdit"
                    :before-save="beforeSave"
                    :parent="parent"
                    :childLinks="childLinks">
        <template v-slot:item.text="{ item }">
            {{ stripHtml(item.text) | truncate(200) }}
        </template>

        <template v-slot:editor="{ item }">
            <v-row>
                <VeeTextField v-model="item.title"
                              :label="mainHeaders.filter(h => h.value == 'title')[0].text"
                              rules="required|max:100"
                              :counter="100" />
            </v-row>
            <v-row>
                <v-switch v-model="preview"
                          label="HTML" />
            </v-row>
            <v-row>
                <VeeRichTextArea v-if="!preview"
                                 v-model="item.text"
                                 label="Text"
                                 rules="required|min:8|max:1000" />
                <VeeTextArea v-else
                             v-model="item.text"
                             label="Text"
                             rules="required|min:1|max:1000" />
            </v-row>
            <v-row>
                <VeeSelect v-model="item.type"
                           :items="types"
                           label="Type"
                           rules="required" />
            </v-row>
            <v-row>
                <v-col>
                    <VeeCheckList title="Diets"
                                  :availableItems="dietList"
                                  :checkedItems="item.diets" />
                </v-col>
                <v-col>
                    <VeeCheckList title="Indications"
                                  :availableItems="indicationList"
                                  :checkedItems="item.indications" />
                </v-col>
                <v-col>
                    <VeeCheckList title="Motivations"
                                  :availableItems="motivationList"
                                  :checkedItems="item.motivations" />
                </v-col>
            </v-row>
        </template>
    </ColabDataTable>
</template>

<script>
    import Vue from 'vue'
    import integration from "@/integration";
    import MethodType from "@/enums/MethodType";
import { toggle } from '../../utils/vuex';

    export default {
        components: {
            ColabDataTable: () => import("@/components/signalR/ColabRDataTable"),
            VeeTextField: () => import("@/components/inputs/VeeTextField"),
            VeeRichTextArea: () => import("@/components/inputs/VeeRichTextArea"),
            VeeTextArea: () => import("@/components/inputs/VeeTextArea"),
            VeeSelect: () => import("@/components/inputs/VeeSelect"),
            VeeCheckList: () => import("@/components/inputs/VeeCheckList")
        },
        data() {
            return {
                mainHeaders: [{
                    sortable: true,
                    value: "title",
                    text: "Title"
                }],
                parent: {
                    path: "Targets",
                    parentIdProperty: "optimizationAreaId",
                    title: "Target",
                    titleProperty: "title",
                    storeName: "targets"
                },
                childLinks: [{
                    path: "Method Multiple Choices",
                    title: "Edit multiple choices",
                    visible: (item) => item.type == 'MultipleChoice'
                }],
                dietList: [],
                indicationList: [],
                motivationList: [],
                preview: false
            }
        },
        async created() {
            this.types = Object.values(MethodType);

            this.dietList = await this.$store.dispatch("dictionaries/getDietList");
            this.indicationList = await this.$store.dispatch("dictionaries/getIndicationList");
            this.motivationList = await this.$store.dispatch("dictionaries/getMotivationList");
        },
        methods: {
            async init(item) {
                if (!item.diets) item.diets = [];
                if (!item.indications) item.indications = [];
                if (!item.motivations) item.motivations = [];
            },
            async beforeEdit(item) {
                this.init(item);
            },
            async beforeSave(item) {
                item.targetId = Number.parseInt(this.$route.params.parentId);
            },
            stripHtml(html) {
                var tmp = document.createElement("div");
                tmp.innerHTML = html;
                return tmp.textContent || tmp.innerText || "";
            }
        }
    }
</script>