<template>
  <ColabDataTable
    title="WebView"
    view-model="WebViewViewModel"
    editor-title-suffix="web view item"
    :headers="mainHeaders"
    :could-add="false"
  >
    <template v-slot:item.Text="{ item }">
      {{ stripHtml(item.Text) | truncate(200) }}
    </template>

    <template v-slot:item.Url="{ item }">
      <a 
        v-if="item.Url"
        :href="item.Url"
        target="_blank"
      >
        {{ item.Url }}
      </a>
    </template>

    <template v-slot:editor="{ item }">
      <v-row>
        <VeeTextField
          v-model="item.Title"
          :label="mainHeaders.filter(h => h.value == 'Title')[0].text"
          rules="required|max:200"
          :counter="200"
        />
      </v-row>
      <v-row v-if="item.Url && item.Url.endsWith('html')">
        <v-switch
          v-model="preview"
          label="Preview HTML"
        />
      </v-row>
      <v-row>
        <VeeRichTextArea
          v-if="!preview && item.Url && item.Url.endsWith('html')"
          v-model="item.Text"
          :label="mainHeaders.filter(h => h.value == 'Text')[0].text"
          rules="required|min:8"
        />
        <VeeTextArea
          v-else
          v-model="item.Text"
          :label="mainHeaders.filter(h => h.value == 'Text')[0].text"
          rules="required"
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
    VeeRichTextArea: () => import("@/components/inputs/VeeRichTextArea"),
  },
  data() {
    return {
      mainHeaders: [{
        sortable: true,
        value: "Title",
        text: "Title"
      }, {
        sortable: false,
        value: "Text",
        text: "Text"
      },
      {
        sortable: false,
        value: "Url",
        text: "Url"
      }],
      preview: false
    }
  },
  methods: {
    stripHtml(html) {
      var tmp = document.createElement("div");
      tmp.innerHTML = html;
      return tmp.textContent || tmp.innerText || "";
    }
  }
}
</script>