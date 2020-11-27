<template>
    <ColabDataTable title="Method Texts"
                    store-name="methodtexts"
                    editor-title-suffix="method text item"
                    :headers="mainHeaders"
                    :before-save="beforeSave"
                    :parent="parent">
        <template v-slot:item.text="{ item }">
            {{ stripHtml(item.text) | truncate(200) }}
        </template>

        <template v-slot:editor="{ item }">
            <v-row>
                <VeeTextField v-model="item.code"
                              :label="mainHeaders.filter(h => h.value == 'code')[0].text"
                              rules="required|max:100"
                              :counter="100" />
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
            VeeTextArea: () => import("@/components/inputs/VeeTextArea")
        },
        data() {
            return {
                mainHeaders: [{
                    sortable: true,
                    value: "code",
                    text: "Code"
                },
                {
                    sortable: true,
                    value: "title",
                    text: "Title"
                }],
                parent: {
                    path: "Methods",
                    title: "Method",
                    titleProperty: "title",
                    storeName: "methods"
                },
                preview: false
            }
        },
        methods: {
            async beforeSave(item) {
                item.methodId = Number.parseInt(this.$route.params.parentId);
            },
            stripHtml(html) {
                var tmp = document.createElement("div");
                tmp.innerHTML = html;
                return tmp.textContent || tmp.innerText || "";
            }
        }
    }
</script>