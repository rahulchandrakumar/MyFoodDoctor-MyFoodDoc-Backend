<template>
    <ColabDataTable title="Questions"
                    store-name="questions"
                    editor-title-suffix="question item"
                    :headers="mainHeaders"
                    :before-save="beforeSave"
                    :parent="parent"
                    :childLinks="childLinks">

        <template v-slot:item.text="{ item }">
            {{ stripHtml(item.text) | truncate(200) }}
        </template>

        <template v-slot:editor="{ item }">
            <v-row>
                <VeeTextArea v-model="item.text"
                             :label="mainHeaders.filter(h => h.value == 'text')[0].text"
                             rules="required|min:1|max:1000" />
            </v-row>
            <v-row>
                <v-switch v-model="item.verticalAlignment"
                          label="Vertical alignment" />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.order"
                              :label="mainHeaders.filter(h => h.value == 'order')[0].text"
                              rules="required|integer|min_value:1"
                              number />
            </v-row>
            <v-row>
                <VeeSelect v-model="item.type"
                           :items="types"
                           :label="mainHeaders.filter(h => h.value == 'type')[0].text"
                           rules="required" />
            </v-row>
            <v-row>
                <v-switch v-model="item.extra"
                          label="Extra question" />
            </v-row>
        </template>
    </ColabDataTable>
</template>

<script>
    import QuestionType from "@/enums/QuestionType";

    export default {
        components: {
            ColabDataTable: () => import("@/components/signalR/ColabRDataTable"),
            VeeTextField: () => import("@/components/inputs/VeeTextField"),
            VeeTextArea: () => import("@/components/inputs/VeeTextArea"),
            VeeSelect: () => import("@/components/inputs/VeeSelect")
        },
        data() {
            return {
                mainHeaders: [{
                    sortable: true,
                    value: "type",
                    text: "Type"
                },{
                    sortable: true,
                    value: "text",
                    text: "Text"
                }, {
                    sortable: true,
                    value: "order",
                    text: "Order"
                }],
                parent: {
                    path: "Scales",
                    title: "Scale",
                    titleProperty: "title",
                    storeName: "scales"
                },
                childLinks: [{
                    path: "Choices",
                    title: "Edit choices",
                }]
            }
        },
        async created() {
            this.types = Object.values(QuestionType);
        },
        methods: {
            async beforeSave(item) {
                item.scaleId = Number.parseInt(this.$route.params.parentId);
            },
            stripHtml(html) {
                var tmp = document.createElement("div");
                tmp.innerHTML = html;
                return tmp.textContent || tmp.innerText || "";
            }
        }
    }
</script>