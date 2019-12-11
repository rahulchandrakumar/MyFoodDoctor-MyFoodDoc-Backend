<template>
  <ValidationProvider
    ref="field"
    v-slot="{ errors, valid }"
    :name="$attrs.label"
    :rules="rules"
    style="display: contents;"
  >
    <v-textarea
      v-model="innerValue"
      :error-messages="errors"
      :success="valid"
      v-bind="$attrs"
      auto-grow
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
