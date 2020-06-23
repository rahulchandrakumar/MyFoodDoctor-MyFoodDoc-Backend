<template>
    <ColabDataTable title="Methods"
                    store-name="methods"
                    editor-title-suffix="method item"
                    :headers="mainHeaders"
                    :before-add="init"
                    :before-edit="beforeEdit"
                    :before-save="beforeSave"
                    :childLinks="childLinks">
        <template v-slot:item.text="{ item }">
            {{ stripHtml(item.text) | truncate(200) }}
        </template>
        <template v-slot:item.image="{ item }">
            <v-img v-if="item.image != null"
                   :src="item.image.Url"
                   contain="true"
                   height="70px"
                   width="210px" />
        </template>
        <template v-slot:editor="{ item }">
            <v-row>
                <VeeImage v-if="squareImage"
                          v-model="item.image"
                          :label="mainHeaders.filter(h => h.value == 'image')[0].text"
                          rules="required"
                          :image-width="300"
                          :image-height="300" />
                <VeeImage v-else
                          v-model="item.image"
                          :label="mainHeaders.filter(h => h.value == 'image')[0].text"
                          rules="required"
                          :image-width="900"
                          :image-height="300" />

            </v-row>
            <v-row>
                <v-switch v-model="squareImage"
                          label="Square" />
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
                           label="Type"
                           rules="required" />
            </v-row>
            <v-row>
                <v-col>
                    <VeeCheckList title="Targets"
                                  :availableItems="targetList"
                                  :checkedItems="item.targets"
                                  labelField="title" />
                </v-col>
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
            VeeCheckList: () => import("@/components/inputs/VeeCheckList"),
            VeeImage: () => import("@/components/inputs/VeeImage")
        },
        data() {
            return {
                mainHeaders: [{
                    filterable: false,
                    sortable: false,
                    value: "image",
                    text: "Image",
                    width: "210px"
                },{
                    sortable: true,
                    value: "title",
                    text: "Title"
                }],
                childLinks: [{
                    path: "Method Multiple Choices",
                    title: "Edit multiple choices",
                    visible: (item) => item.type == 'MultipleChoice'
                }],
                targetList: [],
                dietList: [],
                indicationList: [],
                motivationList: [],
                preview: false,
                squareImage: false
            }
        },
        async created() {
            this.types = Object.values(MethodType);

            this.targetList = await this.$store.dispatch("dictionaries/getTargetList");
            this.dietList = await this.$store.dispatch("dictionaries/getDietList");
            this.indicationList = await this.$store.dispatch("dictionaries/getIndicationList");
            this.motivationList = await this.$store.dispatch("dictionaries/getMotivationList");
        },
        methods: {
            async init(item) {
                if (!item.targets) item.targets = [];
                if (!item.diets) item.diets = [];
                if (!item.indications) item.indications = [];
                if (!item.motivations) item.motivations = [];
            },
            async beforeEdit(item) {
                this.init(item);
            },
            async beforeSave(item) {
                if (item.image && item.image.Url && !item.image.Url.startsWith('http'))
                    item.image = Object.assign(item.image, await integration.images.uploadImage(item.image.Url));
            },
            stripHtml(html) {
                var tmp = document.createElement("div");
                tmp.innerHTML = html;
                return tmp.textContent || tmp.innerText || "";
            }
        }
    }
</script>