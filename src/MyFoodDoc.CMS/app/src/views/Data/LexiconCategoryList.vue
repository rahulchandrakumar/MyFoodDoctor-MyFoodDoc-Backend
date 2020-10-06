<template>
    <ColabDataTable title="Lexicon Categories"
                    store-name="lexiconcategories"
                    editor-title-suffix="lexicon category item"
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
                }],
                childLinks: [{
                    path: "Lexicon",
                    title: "Edit entries",
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