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
        <template v-slot:item.image="{ item }">
            <v-img v-if="item.image != null"
                   :src="item.image.Url"
                   contain
                   height="70px"
                   width="210px" />
        </template>
        <template v-slot:editor="{ item }">
            <v-row v-if="item.type != 'Mood' && item.type != 'Timer'">
                <VeeImage v-if="item.type == 'Meals' || item.type == 'Knowledge'"
                          v-model="item.image"
                          :label="mainHeaders.filter(h => h.value == 'image')[0].text"
                          rules="required"
                          :image-width="900"
                          :image-height="300" />
                <VeeImage v-else
                          v-model="item.image"
                          :label="mainHeaders.filter(h => h.value == 'image')[0].text"
                          rules="required"
                          :image-width="300"
                          :image-height="300" />
            </v-row>
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
                           :label="mainHeaders.filter(h => h.value == 'type')[0].text"
                           rules="required" />
            </v-row>
            <v-row v-if="item.type && item.type != 'Change' && item.type != 'Drink' && item.type != 'Meals' && item.type != 'Timer'">
                <VeeTextField v-model="item.frequency"
                              label="Frequency"
                              rules="integer|min_value:1"
                              number />
                <VeeSelect v-model="item.frequencyPeriod"
                           :items="methodFrequencyPeriods"
                           label="Frequency period" />
            </v-row>
            <v-row v-if="item.type && item.type == 'Timer'">
                <VeeTextField v-model="item.timeIntervalDay"
                              label="Time interval(day), min"
                              rules="integer|min_value:1"
                              number />
                <VeeTextField v-model="item.timeIntervalNight"
                              label="Time interval(night), min"
                              rules="integer|min_value:1"
                              number />
            </v-row>
            <v-row v-if="item.type && item.type != 'Information' && item.type != 'Knowledge'">
                <v-container>
                    <v-layout justify-center row wrap>
                        <v-flex v-if="item.type && (item.type == 'Change' || item.type == 'Drink' || item.type == 'Meals' || item.type == 'Timer')">
                            <v-row>
                                <v-col>
                                    <span style="font-weight: bold">Targets</span>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col>
                                    <VeeCheckList :availableItems="targetList"
                                                  :checkedItems="item.targets"
                                                  labelField="title" />
                                </v-col>
                            </v-row>
                        </v-flex>
                        <v-flex>
                            <v-row>
                                <v-col>
                                    <div style="text-align: center">
                                        <span style="font-weight: bold">Diets</span>
                                    </div>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col>
                                    <VeeCheckList icon="mdi-thumb-up-outline"
                                                  :availableItems="dietList"
                                                  :checkedItems="item.diets" />
                                </v-col>
                                <v-col>
                                    <VeeCheckList icon="mdi-thumb-down-outline"
                                                  :availableItems="dietList"
                                                  :checkedItems="item.contraindicatedDiets" />
                                </v-col>
                            </v-row>
                        </v-flex>
                        <v-flex>
                            <v-row>
                                <v-col>
                                    <div style="text-align: center">
                                        <span style="font-weight: bold">Indications</span>
                                    </div>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col>
                                    <VeeCheckList icon="mdi-thumb-up-outline"
                                                  :availableItems="indicationList"
                                                  :checkedItems="item.indications" />
                                </v-col>
                                <v-col>
                                    <VeeCheckList icon="mdi-thumb-down-outline"
                                                  :availableItems="indicationList"
                                                  :checkedItems="item.contraindications" />
                                </v-col>
                            </v-row>
                        </v-flex>
                        <v-flex>
                            <v-row>
                                <v-col>
                                    <div style="text-align: center">
                                        <span style="font-weight: bold">Motivations</span>
                                    </div>
                                </v-col>
                            </v-row>
                            <v-row>
                                <v-col>
                                    <VeeCheckList icon="mdi-thumb-up-outline"
                                                  :availableItems="motivationList"
                                                  :checkedItems="item.motivations" />
                                </v-col>
                                <v-col>
                                    <VeeCheckList icon="mdi-thumb-down-outline"
                                                  :availableItems="motivationList"
                                                  :checkedItems="item.contraindicatedMotivations" />
                                </v-col>
                            </v-row>
                        </v-flex>
                    </v-layout>
                </v-container>
            </v-row>
        </template>
    </ColabDataTable>
</template>

<script>
    import Vue from 'vue'
    import integration from "@/integration";
    import MethodType from "@/enums/MethodType";
    import MethodFrequencyPeriod from "@/enums/MethodFrequencyPeriod";
    import { toggle } from '../../utils/vuex';

    export default {
        components: {
            ColabDataTable: () => import("@/components/signalR/ColabRDataTable"),
            VeeTextField: () => import("@/components/inputs/VeeTextField"),
            VeeRichTextArea: () => import("@/components/inputs/VeeRichTextArea"),
            VeeTextArea: () => import("@/components/inputs/VeeTextArea"),
            VeeSelect: () => import("@/components/inputs/VeeSelect"),
            VeeCheckList: () => import("@/components/inputs/VeeCheckList"),
            VeeImage: () => import("@/components/inputs/VeeImage")
        },
        data() {

            var result = {
                mainHeaders: [{
                    filterable: false,
                    sortable: false,
                    value: "image",
                    text: "Image",
                    width: "210px"
                }, {
                    sortable: true,
                    value: "type",
                    text: "Type"
                }, {
                    sortable: true,
                    value: "title",
                    text: "Title"
                }],
                parent: null,
                childLinks: [{
                    path: "Methods",
                    title: "Edit methods",
                    visible: (item) => item.type == 'Change' || item.type == 'Drink' || item.type == 'Meals'
                }, {
                    path: "Method Multiple Choices",
                    title: "Edit multiple choices",
                    visible: (item) => item.type == 'Knowledge'
                }, {
                    path: "Method Texts",
                    title: "Edit texts",
                    visible: (item) => item.type == 'Timer'
                }],
                targetList: [],
                dietList: [],
                indicationList: [],
                motivationList: [],
                preview: false
            }

            if (this.$route.params != null && this.$route.params.parentId != null) {
                result.parent = {
                    path: "Generic methods",
                    title: "Method",
                    titleProperty: "title",
                    storeName: "methods"
                };
            }

            return result;
        },
        async created() {
            this.types = Object.values(MethodType);

            if (this.$route.params != null && this.$route.params.parentId != null) {
                this.types = [MethodType.INFORMATION, MethodType.KNOWLEDGE];
            }
            else {
                this.types = Object.values(MethodType).filter((item) => item != MethodType.INFORMATION && item != MethodType.KNOWLEDGE);
            }

            this.methodFrequencyPeriods = [''];
            this.methodFrequencyPeriods = this.methodFrequencyPeriods.concat(Object.values(MethodFrequencyPeriod));

            this.targetList = await this.$store.dispatch("dictionaries/getTargetList");
            this.dietList = await this.$store.dispatch("dictionaries/getDietList");
            this.indicationList = await this.$store.dispatch("dictionaries/getIndicationList");
            this.motivationList = await this.$store.dispatch("dictionaries/getMotivationList");
        },
        methods: {
            async init(item) {
                if (!item.targets) item.targets = [];
                if (!item.diets) item.diets = [];
                if (!item.contraindicatedDiets) item.contraindicatedDiets = [];
                if (!item.indications) item.indications = [];
                if (!item.contraindications) item.contraindications = [];
                if (!item.motivations) item.motivations = [];
                if (!item.contraindicatedMotivations) item.contraindicatedMotivations = [];
            },
            async beforeEdit(item) {
                this.init(item);
            },
            async beforeSave(item) {
                if (item.type == 'Mood' || item.type == 'Timer')
                    item.image = null;
                else if (item.image && item.image.Url && !item.image.Url.startsWith('http'))
                    item.image = Object.assign(item.image, await integration.images.uploadImage(item.image.Url));

                if (item.type == 'Change' || item.type == 'Drink' || item.type == 'Meals' || item.type == 'Timer') {
                    item.frequency = null;
                    item.frequencyPeriod = null;
                } else {
                    if (!item.frequency || item.frequency < 1 || !item.frequencyPeriod) {
                        item.frequency = null;
                        item.frequencyPeriod = null;
                    }

                    item.targets = [];
                }             

                item.parentId = Number.parseInt(this.$route.params.parentId);
            },
            stripHtml(html) {
                var tmp = document.createElement("div");
                tmp.innerHTML = html;
                return tmp.textContent || tmp.innerText || "";
            }
        }
    }
</script>