<template>
  <v-container>
    <v-row>
      <label class="tiptap--label" :class="success ? 'success--text' : 'error--text'">
        {{ label }}
      </label>
    </v-row>
    <v-row>
      <tiptap-vuetify v-model="internalValue" :extensions="extensions" :toolbar-attributes="{ color: success ? 'green' : 'red' }" />
    </v-row>
    <template v-for="(e,i) in errors">
      <v-row :key="i">
        <label class="tiptap--label error--text">
          {{ e }}
        </label>
      </v-row>
    </template>
  </v-container>
</template>

<script>
import {
  // component
  TiptapVuetify,
  // extensions
  Heading,
  Bold,
  Italic,
  Strike,
  Underline,
  Paragraph,
  HardBreak,
  History,
} from "tiptap-vuetify";

export default { 
  name: "VTiptapArea",
  components: { TiptapVuetify },
  props: {
    value: {
      type: null,
      default: null
    },
    label: {
      type: String,
      default: ''
    },
    success: {
      type: Boolean,
      default: false
    },
    errors: {
      type: Array,
      default: () => []
    }
  },
  data() {
    return {
      internalValue: this.value,
      extensions: [
        History,
        Underline,
        Strike,
        Bold,
        Italic,
        [
          Heading,
          {
            // Options that fall into the tiptap's extension
            options: {
              levels: [1]
            }
          }
        ],
        Paragraph,
        HardBreak // line break on Shift + Ctrl + Enter
      ]
    }
  },
  watch: {
    value(newVal) {
      this.internalValue = newVal;
    },
    internalValue(newVal) {
      this.$emit("input", newVal);
    }
  },
}
</script>

<style lang="scss">
.tiptap--label {
  font-size: 12px;
}

.tiptap-vuetify-editor {
  width: 100%;

  .v-toolbar {
    min-height: unset;

    .v-toolbar__content {
      min-height: unset;

      .v-btn.v-btn--icon {
        height: auto;
        margin: 3px
      }
    }
  }
}
</style>