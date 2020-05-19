<template>
    <ColabDataTable title="Optimization Areas"
                    store-name="optimizationareas"
                    editor-title-suffix="optimization area item"
                    :headers="mainHeaders"
                    :before-save="beforeSave"
                    :could-add="false"
                    :could-remove="false">
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
                <VeeTextField v-model="item.upperLimit"
                              label="Upper limit"
                              rules="decimal" />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.lowerLimit"
                              label="Lower limit"
                              rules="decimal" />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.optimal"
                              label="Optimal"
                              rules="decimal" />
            </v-row>
            <v-row v-if="item.id != null">
                <v-btn color="blue darken-1" text @click="editTargets(item)">Edit targets</v-btn>
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
                preview: false
            }
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
            },
            editTargets(item) {
                this.$router.push({ name: 'Targets', params: { parentId: item.id } });
            }
        }
    }
</script>