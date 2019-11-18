<template>
  <v-container>
    <v-row>
      <label class="tiptap--label" :class="success ? 'success--text' : 'error--text'">
        {{ label }}
      </label>
    </v-row>
    <v-row v-if="value == null || !value.startsWith('http')">
      <v-image-input
        :key="key"
        :debounce="300"
        v-model="innerValue"        
        class="v-image-input"
        :image-quality="0.65"
        image-format="jpeg"
        :image-height="imageHeight"
        :image-width="imageWidth"
        overlay-padding="20px"
        :style="{ 'min-width': (imageWidth + 20) + 'px' }"
        clearable
      />
    </v-row>
    <v-row v-else class="align-start v-image-display">
      <v-img
        :src="innerValue"
        :aspect-ratio="imageWidth + ':' + imageHeight"
        :height="imageHeight"
        :max-height="imageHeight"
        :width="imageWidth"
        :max-width="imageWidth"
      />
      <v-btn text icon @click="innerValue = null">
        <v-icon>
          mdi-close
        </v-icon>
      </v-btn>
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
import VImageInput from 'vuetify-image-input';

export default {
  components: {
    [VImageInput.name]: VImageInput,
  },
  props: {
    value: {
      type: null,
      default: null
    },
    label: {
      type: String,
      default: ''
    },
    imageHeight: {
      type: Number,
      default: 100
    },
    imageWidth: {
      type: Number,
      default: 100
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
      innerValue: this.value,
      key: false
    }
  },
  watch: {
    innerValue(newVal) {
      this.$emit("input", newVal);
    },
    value(newVal) {
      this.innerValue = newVal;
      if (newVal == "")
        this.key = !this.key;
    }
  },
}
</script>

<style lang="scss">
.v-image-input {
  flex: auto;
}
.v-image-input, .v-image-display {
  .v-btn {
    color: black !important;
    .v-btn__content {
      color: inherit;
      i.v-icon {
        color: inherit;
      }
    }
  }
}
.v-image-display {
  margin-bottom: 40px;
  .v-image {
    margin: 24px 27px 24px 27px;
  }
}
</style>