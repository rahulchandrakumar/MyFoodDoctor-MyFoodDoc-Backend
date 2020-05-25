<template>
    <ColabDataTable title="Chapters"
                    store-name="chapters"
                    editor-title-suffix="chapter item"
                    :headers="mainHeaders"
                    :before-save="beforeSave"
                    :parent="parent"
                    :childLinks="childLinks">
        <template v-slot:item.text="{ item }">
            {{ stripHtml(item.text) | truncate(200) }}
        </template>
        <template v-slot:item.questionText="{ item }">
            {{ stripHtml(item.questionText) | truncate(200) }}
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
                          :image-width="900"
                          :image-height="300" />
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
                <VeeTextField v-model="item.order"
                              :label="mainHeaders.filter(h => h.value == 'order')[0].text"
                              rules="required|integer|min_value:1"
                              number />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.questionTitle"
                              label="Question title"
                              rules="required|max:100"
                              :counter="100" />
            </v-row>
            <v-row>
                <VeeRichTextArea v-if="!preview"
                                 v-model="item.questionText"
                                 label="Question text"
                                 rules="required|min:8|max:1000" />
                <VeeTextArea v-else
                             v-model="item.questionText"
                             label="Question text"
                             rules="required|min:1|max:1000" />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.answerText1"
                              label="Answer 1"
                              rules="required|max:100"
                              :counter="100" />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.answerText2"
                              label="Answer 2"
                              rules="required|max:100"
                              :counter="100" />
            </v-row>
            <v-row>
                <v-switch v-model="item.answer"
                          label="Correct answer(1/2)" />
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
                },
                {
                    sortable: true,
                    value: "title",
                    text: "Title"
                }, {
                    sortable: true,
                    value: "order",
                    text: "Order"
                }],
                parent: {
                    path: "Courses",
                    title: "Course",
                    titleProperty: "title",
                    storeName: "courses"
                },
                childLinks: [{
                    path: "Subchapters",
                    title: "Edit subchapters",
                }],
                preview: false
            }
        },
        methods: {
            async beforeSave(item) {
                if (item.image && item.image.Url && !item.image.Url.startsWith('http'))
                    item.image = Object.assign(item.image, await integration.images.uploadImage(item.image.Url));

                item.courseId = Number.parseInt(this.$route.params.parentId);
            },
            stripHtml(html) {
                var tmp = document.createElement("div");
                tmp.innerHTML = html;
                return tmp.textContent || tmp.innerText || "";
            }
        }
    }
</script>