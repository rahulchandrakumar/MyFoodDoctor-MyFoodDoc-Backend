<template>
    <ColabDataTable title="Method Multiple Choices"
                    store-name="methodmultiplechoices"
                    editor-title-suffix="method multiple choice item"
                    :headers="mainHeaders"
                    :before-save="beforeSave"
                    :parent="parent">
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
                <v-checkbox v-model="item.isCorrect"
                          :label="mainHeaders.filter(h => h.value == 'isCorrect')[0].text" />
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
            VeeTextField: () => import("@/components/inputs/VeeTextField")
        },
        data() {
            return {
                mainHeaders: [{
                    sortable: true,
                    value: "title",
                    text: "Title"
                },
                {
                    sortable: true,
                    value: "isCorrect",
                    text: "Correct"
                }],
                parent: {
                    path: "Methods",
                    parentIdProperty: "targetId",
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