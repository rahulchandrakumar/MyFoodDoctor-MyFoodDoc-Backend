<template>
    <ColabDataTable title="Choices"
                    store-name="choices"
                    editor-title-suffix="choice item"
                    :headers="mainHeaders"
                    :before-save="beforeSave"
                    :parent="parent">

        <template v-slot:editor="{ item }">
            <v-row>
                <VeeTextField v-model="item.text"
                              :label="mainHeaders.filter(h => h.value == 'text')[0].text"
                              rules="required|max:100"
                              :counter="100" />
            </v-row>
            <v-row>
                <v-switch v-model="item.scorable"
                          :label="mainHeaders.filter(h => h.value == 'scorable')[0].text" />
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
    export default {
        components: {
            ColabDataTable: () => import("@/components/signalR/ColabRDataTable"),
            VeeTextField: () => import("@/components/inputs/VeeTextField")
        },
        data() {
            return {
                mainHeaders: [{
                    sortable: true,
                    value: "text",
                    text: "Text"
                }, {
                    sortable: true,
                    value: "scorable",
                    text: "Scorable"
                }, {
                    sortable: true,
                    value: "order",
                    text: "Order"
                }],
                parent: {
                    path: "Questions",
                    parentIdProperty: "scaleId",
                    title: "Question",
                    titleProperty: "text",
                    storeName: "questions"
                }
            }
        },
        methods: {
            async beforeSave(item) {
                item.questionId = Number.parseInt(this.$route.params.parentId);
            }
        }
    }
</script>