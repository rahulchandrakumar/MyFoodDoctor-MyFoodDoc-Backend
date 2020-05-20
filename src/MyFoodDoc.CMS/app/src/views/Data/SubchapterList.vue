<template>
    <ColabDataTable title="Subchapters"
                    store-name="subchapters"
                    editor-title-suffix="subchapter item"
                    :headers="mainHeaders"
                    :before-save="beforeSave">
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
                <VeeTextField v-model="item.order"
                              :label="mainHeaders.filter(h => h.value == 'order')[0].text"
                              rules="required|integer|min_value:1"
                              number />
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
                    sortable: true,
                    value: "title",
                    text: "Title"
                }, {
                    sortable: true,
                    value: "order",
                    text: "Order"
                }],
                preview: false
            }
        },
        methods: {
            async beforeSave(item) {
                item.chapterId = Number.parseInt(this.$route.params.parentId);
            },
            stripHtml(html) {
                var tmp = document.createElement("div");
                tmp.innerHTML = html;
                return tmp.textContent || tmp.innerText || "";
            }
        }
    }
</script>