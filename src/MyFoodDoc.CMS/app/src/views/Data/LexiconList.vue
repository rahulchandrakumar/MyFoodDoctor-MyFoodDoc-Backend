<template>
  <ColabDataTable
    title="Lexicon"
    view-model="LexiconViewModel"
    editor-title-suffix="lexicon item"
    :headers="mainHeaders"
    :before-save="beforeSave"
  >
    <template v-slot:item.Text="{ item }">
      {{ stripHtml(item.Text) | truncate(200) }}
    </template>
    <template v-slot:item.Image="{ item }">
      <v-img 
        v-if="item.Image != null"
        :aspect-ratio="3/1" 
        :src="item.Image.Url" 
        height="70px"         
      />
    </template>

    <template v-slot:editor="{ item }">
      <v-row>
        <VeeImage
          v-model="item.Image"
          :label="mainHeaders.filter(h => h.value == 'Image')[0].text"            
          rules="required"
          :image-width="900" 
          :image-height="300"
        />
      </v-row>
      <v-row>
        <VeeTextField
          v-model="item.TitleShort"
          :label="mainHeaders.filter(h => h.value == 'TitleShort')[0].text"
          rules="required|max:30"
          :counter="30"
        />
      </v-row>
      <v-row>
        <VeeTextField
          v-model="item.TitleLong"
          :label="mainHeaders.filter(h => h.value == 'TitleLong')[0].text"
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
          v-model="item.Text"
          :label="mainHeaders.filter(h => h.value == 'Text')[0].text"
          rules="required|min:8|max:8192"
        />
        <VeeTextArea
          v-else
          v-model="item.Text"
          :label="mainHeaders.filter(h => h.value == 'Text')[0].text"
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
    ColabDataTable: () => import("@/components/dotnetify/ColabDataTable"),
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
        value: "Image",
        text: "Image",
        width: "210px"
      }, {
        sortable: true,
        value: "TitleShort",
        text: "Title (short)"
      }, {
        sortable: true,
        value: "TitleLong",
        text: "Title (long)"
      }, {
        sortable: false,
        value: "Text",
        text: "Description"
      }],
      preview: false
    }
  },
  methods: {
    async beforeSave(item) {
      if (item.Image.Url != null && !item.Image.Url.startsWith('http'))
        item.Image = Object.assign(item.Image, await integration.images.uploadImage(item.Image.Url));
    },
    stripHtml(html) {
      var tmp = document.createElement("div");
      tmp.innerHTML = html;
      return tmp.textContent || tmp.innerText || "";
    }
  }
}
</script>