<template>
  <ValidationProvider :name="$attrs.label" :rules="rules" v-slot="{ errors, valid }" style="display: contents;">
    <v-text-field
        v-model="innerValue"
        :error-messages="errors"
        :success="valid"
        v-bind="$attrs"
        v-on="$listeners"
    ></v-text-field>
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
      type: null
    }
  },
  data: () => ({
    innerValue: ""
  }),
  watch: {
    innerValue(newVal) {
      this.$emit("input", newVal);
    },
    value(newVal) {
      this.innerValue = newVal;
    }
  },
  created() {
    if (this.value) {
      this.innerValue = this.value;
    }
  }
};
</script>
