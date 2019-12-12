<template>
  <ColabDataTable
    title="Lexicon"
    store-name="lexicon"
    editor-title-suffix="lexicon item"
    :headers="mainHeaders"
    :before-save="beforeSave"
  >
    <template v-slot:item.text="{ item }">
      {{ stripHtml(item.text) | truncate(200) }}
    </template>
    <template v-slot:item.image="{ item }">
      <v-img 
        v-if="item.image != null"
        :aspect-ratio="3/1" 
        :src="item.image.Url" 
        height="70px"         
      />
    </template>

    <template v-slot:editor="{ item }">
      <v-row>
        <VeeImage
          v-model="item.image"
          :label="mainHeaders.filter(h => h.value == 'image')[0].text"            
          rules="required"
          :image-width="900" 
          :image-height="300"
        />
      </v-row>
      <v-row>
        <VeeTextField
          v-model="item.titleShort"
          :label="mainHeaders.filter(h => h.value == 'titleShort')[0].text"
          rules="required|max:30"
          :counter="30"
        />
      </v-row>
      <v-row>
        <VeeTextField
          v-model="item.titleLong"
          :label="mainHeaders.filter(h => h.value == 'titleLong')[0].text"
          rules="required|max:200"
          :counter="200"
        />
      </v-row>
      <v-row>
        <v-switch
          v-model="preview"
          label="HTML"
        />
      </v-row>
      <v-row>
        <VeeRichTextArea
          v-if="!preview"
          v-model="item.text"
          :label="mainHeaders.filter(h => h.value == 'text')[0].text"
          rules="required|min:8|max:8192"
        />
        <VeeTextArea
          v-else
          v-model="item.text"
          :label="mainHeaders.filter(h => h.value == 'text')[0].text"
          rules="required|min:1|max:8192"
        />
      </v-row>
    </template>
  </ColabDataTable>
</template>

<script>
import Vue from 'vue'
import integration from "@/integration";

export default {
  components: {
    ColabDataTable: () => import("@/components/dotnetify/ColabRDataTable"),
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
        value: "titleShort",
        text: "Title (short)"
      }, {
        sortable: true,
        value: "titleLong",
        text: "Title (long)"
      }, {
        sortable: false,
        value: "text",
        text: "Description"
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
    }
  }
}
</script>