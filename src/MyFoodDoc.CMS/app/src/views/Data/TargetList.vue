<template>
    <ColabDataTable title="Targets"
                    store-name="targets"
                    editor-title-suffix="target item"
                    :headers="mainHeaders"
                    :before-save="beforeSave"
                    :could-add="false"
                    :could-remove="false">
        <template v-slot:item.text="{ item }">
            {{ stripHtml(item.text) | truncate(200) }}
        </template>
        <template v-slot:item.image="{ item }">
            <v-img v-if="item.image != null && item.type == 'Adjustment'"
                   :aspect-ratio="3/1"
                   :src="item.image.Url"
                   height="70px" />
            <v-img v-else-if="item.image != null"
                   :aspect-ratio="1/1"
                   :src="item.image.Url"
                   height="70px"
                   width="70px" />
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
                <VeeTextArea v-model="item.text"
                             :label="mainHeaders.filter(h => h.value == 'text')[0].text"
                             rules="required|max:1000"
                             :counter="1000" />
            </v-row>
            <v-row>
                <!--
                <VeeSelect v-model="item.type"
                           :items="types"
                           label="Typ"
                           rules="required"
                           disabled="true"
                           readonly />
                    -->
                <VeeTextField v-model="item.type"
                              label="Typ"
                              rules="required"
                              readonly />
            </v-row>
            <v-row>
                <VeeSelect v-model="item.priority"
                           :items="priorities"
                           label="Priorität"
                           rules="required" />
            </v-row>
            <v-row>
                <VeeSelect v-model="item.triggerOperator"
                           :items="operators"
                           label="Auslöseoperator"
                           rules="required" />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.triggerValue"
                              label="Auslösewert"
                              rules="required|decimal"
                              number />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.threshold"
                              label="Fälle,%"
                              rules="required|decimal"
                              number />
            </v-row>
            <v-row v-if="item.type == 'Adjustment'">
                <VeeTextField v-model="item.targetValue"
                              label="Zielwert"
                              rules="required|decimal"
                              number />
            </v-row>
            <v-row v-if="item.type == 'Adjustment'">
                <VeeTextField v-model="item.step"
                              label="Interval/Schritte"
                              rules="required|decimal"
                              number />
            </v-row>
            <v-row v-if="item.type == 'Adjustment'">
                <VeeSelect v-model="item.stepDirection"
                           :items="stepDirections"
                           label="Intervalsteigung"
                           rules="required" />
            </v-row>
            <v-row v-if="item.type == 'Adjustment'">
                <VeeTextField v-model="item.recommendedText"
                              label="Antwort(empfohlen)"
                              rules="required|max:1000"
                              :counter="1000" />
            </v-row>
            <v-row v-if="item.type == 'Adjustment'">
                <VeeTextField v-model="item.targetText"
                              label="Antwort(Ziel)"
                              rules="required|max:1000"
                              :counter="1000" />
            </v-row>
            <v-row v-if="item.type == 'Adjustment'">
                <VeeTextField v-model="item.remainText"
                              label="Antwort(Beibehalten)"
                              rules="required|max:1000"
                              :counter="1000" />
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
            VeeTextArea: () => import("@/components/inputs/VeeTextArea"),
            VeeSelect: () => import("@/components/inputs/VeeSelect"),
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
                }, {
                    sortable: true,
                    value: "title",
                    text: "Title"
                }, {
                    sortable: false,
                    value: "text",
                    text: "Text"
                }],
                preview: false,
                priorities: [],
                types: [],
                operators: [],
                stepDirections: []
            }
        },
        created() {
            this.priorities = Object.values(TargetPriority);
            this.types = Object.values(TargetType);
            this.operators = Object.values(TriggerOperator);
            this.stepDirections = Object.values(AdjustmentTargetStepDirection);
        },
        methods: {
            async beforeSave(item) {
                if (item.image.Url != null && !item.image.Url.startsWith('http'))
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