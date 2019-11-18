<template>
  <ValidationProvider
    v-slot="{ errors, valid }"
    :name="$attrs.label"
    :rules="rules"
    style="display: contents;"
    mode="passive"
    ref="field"
  >
    <v-tiptap-area
      v-model="innerValue"
      :errors="errors"
      :success="valid"
      v-bind="$attrs"
      v-on="$listeners"
    />
  </ValidationProvider>
</template>

<script>
export default {
  props: {
    rules: {
      type: String,
      default: ""
    },
    value: {
      type: null,
      default: null
    }
  },
  data() {
    return {
      innerValue: this.value
    }
  },
  watch: {
    innerValue(newVal) {
      this.$emit("input", newVal);
    },
    value(newVal) {
      this.innerValue = newVal;
            
      let self = this
      setTimeout(() => self.$refs.field.validate(), 10)
    }
  },
  mounted() {
    this.$refs.field.validate()
  }
};
</script>
