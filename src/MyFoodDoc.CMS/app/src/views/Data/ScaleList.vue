<template>
    <ColabDataTable title="Scales"
                    store-name="scales"
                    editor-title-suffix="scale item"
                    :headers="mainHeaders"
                    :before-save="beforeSave"
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
                                 :label="mainHeaders.filter(h => h.value == 'text')[0].text"
                                 rules="required|min:8|max:1000" />
                <VeeTextArea v-else
                             v-model="item.text"
                             :label="mainHeaders.filter(h => h.value == 'text')[0].text"
                             rules="required|min:1|max:1000" />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.order"
                              :label="mainHeaders.filter(h => h.value == 'order')[0].text"
                              rules="required|integer|min_value:1"
                              number />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.typeCode"
                              label="Type code"
                              rules="required|max:6"
                              :counter="6" />
            </v-row>
            <v-row>
                <VeeTextField v-model="item.typeTitle"
                              label="Type title"
                              rules="required|max:100"
                              :counter="100" />
            </v-row>
            <v-row>
                <VeeRichTextArea v-if="!preview"
                                 v-model="item.typeText"
                                 label="Type text"
                                 rules="required|min:8|max:1000" />
                <VeeTextArea v-else
                             v-model="item.typeText"
                             label="Type text"
                             rules="required|min:1|max:1000" />
            </v-row>
            <v-row>
                <VeeRichTextArea v-if="!preview"
                                 v-model="item.characterization"
                                 label="Characterization"
                                 rules="required|min:8|max:1000" />
                <VeeTextArea v-else
                             v-model="item.characterization"
                             label="Characterization"
                             rules="required|min:1|max:1000" />
            </v-row>
            <v-row>
                <VeeRichTextArea v-if="!preview"
                                 v-model="item.reason"
                                 label="Reason"
                                 rules="required|min:8|max:1000" />
                <VeeTextArea v-else
                             v-model="item.reason"
                             label="Reason"
                             rules="required|min:1|max:1000" />
            </v-row>
            <v-row>
                <VeeRichTextArea v-if="!preview"
                                 v-model="item.treatment"
                                 label="Treatment"
                                 rules="required|min:8|max:1000" />
                <VeeTextArea v-else
                             v-model="item.treatment"
                             label="Treatment"
                             rules="required|min:1|max:1000" />
            </v-row>
        </template>
    </ColabDataTable>
</template>

<script>
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
                    value: "title",
                    text: "Title"
                }, {
                    sortable: true,
                    value: "text",
                    text: "Text"
                }, {
                    sortable: true,
                    value: "order",
                    text: "Order"
                }],
                childLinks: [{
                    path: "Questions",
                    title: "Edit questions",
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