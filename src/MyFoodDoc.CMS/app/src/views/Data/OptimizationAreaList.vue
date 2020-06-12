<template>
    <ColabDataTable title="Optimization Areas"
                    store-name="optimizationareas"
                    editor-title-suffix="optimization area item"
                    :headers="mainHeaders"
                    :before-save="beforeSave"
                    :could-add="false"
                    :could-remove="false"
                    :childLinks="childLinks">
        <template v-slot:item.text="{ item }">
            {{ stripHtml(item.text) | truncate(200) }}
        </template>
        <template v-slot:item.image="{ item }">
            <v-img v-if="item.image != null"
                   :aspect-ratio="3/1"
                   :src="item.image.Url"
                   height="70px" />
        </template>

        <template v-slot:editor="{ item }">
            <v-row>
                <VeeImage v-model="item.image"
                          :label="mainHeaders.filter(h => h.value == 'image')[0].text"
                          rules="required"
                          :image-width="900"
                          :image-height="300" />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.key"
                              label="Key"
                              readonly />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.name"
                              :label="mainHeaders.filter(h => h.value == 'name')[0].text"
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
                                 :label="mainHeaders.filter(h => h.value == 'text')[0].text"
                                 rules="required|min:8|max:1000" />
                <VeeTextArea v-else
                             v-model="item.text"
                             :label="mainHeaders.filter(h => h.value == 'text')[0].text"
                             rules="required|min:1|max:1000" />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.lineGraphUpperLimit"
                              label="Line graph upper limit"
                              rules="decimal"
                              number />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.lineGraphLowerLimit"
                              label="Line graph lower limit"
                              rules="decimal"
                              number />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.lineGraphOptimal"
                              label="Line graph optimal"
                              rules="decimal"
                              number />
            </v-row>

            <v-row>
                <VeeTextField v-model="item.optimalLineGraphTitle"
                              label="Optimal line graph title"
                              rules="max:100"
                              :counter="100" />
            </v-row>
            <v-row>
                <VeeRichTextArea v-if="!preview"
                                 v-model="item.optimalLineGraphText"
                                 label="Optimal line graph text"
                                 rules="max:1000" />
                <VeeTextArea v-else
                             v-model="item.optimalLineGraphText"
                             label="Optimal line graph text"
                             rules="max:1000" />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.belowOptimalLineGraphTitle"
                              label="Below optimal line graph title"
                              rules="max:100"
                              :counter="100" />
            </v-row>
            <v-row>
                <VeeRichTextArea v-if="!preview"
                                 v-model="item.belowOptimalLineGraphText"
                                 label="Below optimal line graph text"
                                 rules="max:1000" />
                <VeeTextArea v-else
                             v-model="item.belowOptimalLineGraphText"
                             label="Below optimal line graph text"
                             rules="max:1000" />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.aboveOptimalLineGraphTitle"
                              label="Above optimal line graph title"
                              rules="max:100"
                              :counter="100" />
            </v-row>
            <v-row>
                <VeeRichTextArea v-if="!preview"
                                 v-model="item.aboveOptimalLineGraphText"
                                 label="Above optimal line graph text"
                                 rules="max:1000" />
                <VeeTextArea v-else
                             v-model="item.aboveOptimalLineGraphText"
                             label="Above optimal line graph text"
                             rules="max:1000" />
            </v-row>

            <v-row>
                <VeeTextField v-model="item.OptimalPieChartTitle"
                              label="Optimal pie chart title"
                              rules="max:100"
                              :counter="100" />
            </v-row>
            <v-row>
                <VeeRichTextArea v-if="!preview"
                                 v-model="item.optimalPieChartText"
                                 label="Optimal pie chart text"
                                 rules="max:1000" />
                <VeeTextArea v-else
                             v-model="item.optimalPieChartText"
                             label="Optimal pie chart text"
                             rules="max:1000" />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.belowOptimalPieChartTitle"
                              label="Below optimal pie chart title"
                              rules="max:100"
                              :counter="100" />
            </v-row>
            <v-row>
                <VeeRichTextArea v-if="!preview"
                                 v-model="item.belowOptimalPieChartText"
                                 label="Below optimal pie chart text"
                                 rules="max:1000" />
                <VeeTextArea v-else
                             v-model="item.belowOptimalPieChartText"
                             label="Below optimal pie chart text"
                             rules="max:1000" />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.aboveOptimalPieChartTitle"
                              label="Above optimal pie chart title"
                              rules="max:100"
                              :counter="100" />
            </v-row>
            <v-row>
                <VeeRichTextArea v-if="!preview"
                                 v-model="item.aboveOptimalPieChartText"
                                 label="Above optimal pie chart text"
                                 rules="max:1000" />
                <VeeTextArea v-else
                             v-model="item.aboveOptimalPieChartText"
                             label="Above optimal pie chart text"
                             rules="max:1000" />
            </v-row>
        </template>
    </ColabDataTable>
</template>

<script>
    import Vue from 'vue'
    import integration from "@/integration";

    export default {
        components: {
            ColabDataTable: () => import("@/components/signalR/ColabRDataTable"),
            VeeTextField: () => import("@/components/inputs/VeeTextField"),
            VeeRichTextArea: () => import("@/components/inputs/VeeRichTextArea"),
            VeeTextArea: () => import("@/components/inputs/VeeTextArea"),
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
                    value: "name",
                    text: "Name"
                }, {
                    sortable: false,
                    value: "text",
                    text: "Text"
                }],
                childLinks: [{
                    path: "Targets",
                    title: "Edit targets",
                }],
                preview: false
            }
        },
        methods: {
            async beforeSave(item) {
                if (item.image.Url && !item.image.Url.startsWith('http'))
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