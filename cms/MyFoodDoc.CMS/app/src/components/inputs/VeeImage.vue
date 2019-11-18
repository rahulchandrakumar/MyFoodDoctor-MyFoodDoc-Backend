<template>
  <ValidationProvider
    v-slot="{ errors, valid }"
    :name="$attrs.label"
    :rules="rules"
    style="display: contents;"
    mode="passive"
    ref="field"
  >
    <v-image
      v-model="innerValue"
      :error-messages="errors"
      :success="valid"
      v-bind="$attrs"
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
      innerValue: this.value && this.value.Url ? this.value.Url : new String()
    }
  },
  watch: {
    innerValue(newVal) {
      let change = this.value || {}
      change.ImageData = newVal
      this.$emit("input", change);
    },
    value(newVal) {
      this.innerValue = newVal && newVal.Url ? newVal.Url : new String();
      
      let self = this
      setTimeout(() => self.$refs.field.validate(), 10)
    }
  },
  mounted() {
    this.$refs.field.validate()
  }
}
</script>

<style lang="scss" scoped>

</style>