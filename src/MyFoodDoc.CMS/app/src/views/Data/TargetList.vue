<template>
  <ColabDataTable
    title="Targets"
    store-name="targets"
    editor-title-suffix="target item"
    :headers="mainHeaders"
    :before-save="beforeSave"
  >
    <template v-slot:item.text="{ item }">
      {{ stripHtml(item.text) | truncate(200) }}
    </template>
    <template v-slot:item.image="{ item }">
      <v-img 
        v-if="item.image != null && item.type == 'Adjustment'"
        :aspect-ratio="3/1" 
        :src="item.image.Url" 
        height="70px"         
      />
      <v-img 
        v-else-if="item.image != null"
        :aspect-ratio="1/1" 
        :src="item.image.Url" 
        height="70px"
        width="70px"
      />
    </template>

    <template v-slot:editor="{ item }">
      <v-row>
        <VeeImage
          v-if="item.type == 'Adjustment'"
          v-model="item.image"
          :label="mainHeaders.filter(h => h.value == 'image')[0].text"            
          rules="required"
          :image-width="900" 
          :image-height="300"
        />
        <VeeImage
          v-else
          v-model="item.image"
          :label="mainHeaders.filter(h => h.value == 'image')[0].text"            
          rules="required"
          :image-width="300" 
          :image-height="300"
        />
      </v-row>
      <v-row>
        <VeeTextField
          v-model="item.title"
          :label="mainHeaders.filter(h => h.value == 'title')[0].text"
          rules="required|max:1000"
          :counter="1000"
        />
      </v-row>
      <v-row>
        <VeeTextArea
          v-model="item.text"
          :label="mainHeaders.filter(h => h.value == 'text')[0].text"
          rules="required|max:1000"
          :counter="1000"
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
    ColabDataTable: () => import("@/components/signalR/ColabRDataTable"),
    VeeTextField: () => import("@/components/inputs/VeeTextField"),
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
        sortable: false,
        value: "text",
        text: "Text"
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