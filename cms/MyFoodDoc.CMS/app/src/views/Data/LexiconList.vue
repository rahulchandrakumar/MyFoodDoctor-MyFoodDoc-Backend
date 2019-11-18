<template>
  <ColabDataTable
    title="Lexicon"
    view-model="LexiconViewModel"
    editor-title-suffix="lexicon item"
    :headers="mainHeaders"
    :before-save="beforeSave"
  >
    <template v-slot:item.Description="{ item }">
      {{ item.Description | truncate(150) }}
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
          v-model="item.Title"
          :label="mainHeaders.filter(h => h.value == 'Title')[0].text"
          rules="required|max:35"
          :counter="35"
        />
      </v-row>
      <v-row>
        <VeeTextArea
          v-model="item.Description"
          :label="mainHeaders.filter(h => h.value == 'Description')[0].text"
          rules="required|min:8|max:8192"
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
        value: "Title",
        text: "Title"
      }, {
        sortable: false,
        value: "Description",
        text: "Description"
      }]
    }
  },
  methods: {
    async beforeSave(item) {
      if (item.Image.Url != null && !item.Image.Url.startsWith('http'))
        item.Image = Object.assign(item.Image, await integration.images.uploadImage(item.Image.Url));
    }
  }
}
</script>