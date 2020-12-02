<template>
    <ColabDataTable title="Targets"
                    store-name="targets"
                    editor-title-suffix="target item"
                    :headers="mainHeaders"
                    :before-add="init"
                    :before-edit="beforeEdit"
                    :before-save="beforeSave"
                    :parent="parent">
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
            <v-row>
                <VeeImage v-if="item.type == 'Adjustment'"
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
                              rules="required|max:1000"
                              :counter="1000" />
            </v-row>
            <v-row>
                <v-switch v-model="preview"
                          label="HTML" />
            </v-row>
            <v-row>
                <VeeRichTextArea v-if="!preview"
                                 v-model="item.text"
                                 :label="mainHeaders.filter(h => h.value == 'text')[0].text"
                                 rules="required|min:8|max:1000" />
                <VeeTextArea v-else
                             v-model="item.text"
                             :label="mainHeaders.filter(h => h.value == 'text')[0].text"
                             rules="required|min:1|max:1000" />
            </v-row>
            <v-row>
                <VeeSelect v-model="item.type"
                           :items="types"
                           label="Type"
                           rules="required"
                           readonly />
            </v-row>
            <v-row>
                <VeeSelect v-model="item.priority"
                           :items="priorities"
                           label="Priority"
                           rules="required" />
            </v-row>
            <v-row>
                <VeeSelect v-model="item.triggerOperator"
                           :items="operators"
                           label="Trigger operator"
                           rules="required" />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.triggerValue"
                              label="Trigger value"
                              rules="required|decimal"
                              number />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.threshold"
                              label="Cases, %"
                              rules="required|decimal"
                              number />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.minInterval"
                              label="Min interval between meals, min"
                              rules="integer|min_value:1"
                              number />
            </v-row>
            <v-row v-if="item.type == 'Adjustment'">
                <VeeTextField v-model="item.targetValue"
                              label="Target value"
                              rules="required|decimal"
                              number />
            </v-row>
            <v-row v-if="item.type == 'Adjustment'">
                <VeeTextField v-model="item.step"
                              label="Step"
                              rules="required|decimal"
                              number />
            </v-row>
            <v-row v-if="item.type == 'Adjustment'">
                <VeeSelect v-model="item.stepDirection"
                           :items="stepDirections"
                           label="Step direction"
                           rules="required" />
            </v-row>
            <v-row v-if="item.type == 'Adjustment'">
                <VeeTextField v-model="item.recommendedText"
                              label="Answer(recommended)"
                              rules="required|max:1000"
                              :counter="1000" />
            </v-row>
            <v-row v-if="item.type == 'Adjustment'">
                <VeeTextField v-model="item.targetText"
                              label="Answer(Target)"
                              rules="required|max:1000"
                              :counter="1000" />
            </v-row>
            <v-row v-if="item.type == 'Adjustment'">
                <VeeTextField v-model="item.remainText"
                              label="Anwer(Remain)"
                              rules="required|max:1000"
                              :counter="1000" />
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
    import TargetPriority from "@/enums/TargetPriority";
    import TargetType from "@/enums/TargetType";
    import TriggerOperator from "@/enums/TriggerOperator";
    import AdjustmentTargetStepDirection from "@/enums/AdjustmentTargetStepDirection";

    export default {
        components: {
            ColabDataTable: () => import("@/components/signalR/ColabRDataTable"),
            VeeTextField: () => import("@/components/inputs/VeeTextField"),
            VeeRichTextArea: () => import("@/components/inputs/VeeRichTextArea"),
            VeeTextArea: () => import("@/components/inputs/VeeTextArea"),
            VeeSelect: () => import("@/components/inputs/VeeSelect"),
            VeeImage: () => import("@/components/inputs/VeeImage"),
            VeeCheckList: () => import("@/components/inputs/VeeCheckList")
        },
        data() {
            return {
                mainHeaders: [{
                    filterable: false,
                    sortable: false,
                    value: "image",
                    text: "Image",
                    width: "210px"
                }, {
                    sortable: true,
                    value: "title",
                    text: "Title"
                }, {
                    sortable: false,
                    value: "text",
                    text: "Text"
                }],
                parent: {
                    path: "Optimization Areas",
                    title: "Optimization Area",
                    titleProperty: "name",
                    storeName: "optimizationareas"
                },
                preview: false,
                priorities: [],
                types: [],
                operators: [],
                stepDirections: [],
                dietList: [],
                indicationList: [],
                motivationList: [],
            }
        },
        async created() {
            this.priorities = Object.values(TargetPriority);
            this.types = Object.values(TargetType);
            this.operators = Object.values(TriggerOperator);
            this.stepDirections = Object.values(AdjustmentTargetStepDirection);

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
                if (item.image.Url && !item.image.Url.startsWith('http'))
                    item.image = Object.assign(item.image, await integration.images.uploadImage(item.image.Url));

                item.optimizationAreaId = Number.parseInt(this.$route.params.parentId);
            },
            stripHtml(html) {
                var tmp = document.createElement("div");
                tmp.innerHTML = html;
                return tmp.textContent || tmp.innerText || "";
            }
        }
    }
</script>