<template>
  <v-menu
    v-model="menu"
    :close-on-content-click="false"
    :nudge-right="40"
    transition="scale-transition"
    offset-y
    min-width="290px"
  >
    <template v-slot:activator="{ on }">
      <ValidationProvider
        ref="field"
        v-slot="{ errors, valid }"
        :name="$attrs.label"
        :rules="rules"
        style="display: contents;"
      >
        <v-text-field
          v-model="innerValue"
          :error-messages="errors"
          :success="valid"
          v-bind="$attrs"
          prepend-icon="mdi-calendar"
          readonly
          v-on="on"
        />
      </ValidationProvider>
    </template>
    <v-date-picker v-model="innerValue" @input="menu = false" />
  </v-menu>
</template>

<script>
export default {
  props: {
    rules: {
      type: Object,
      default: null
    },
    value: {
      type: null,
      default: null
    }
  },
  data() {
    return {
      innerValue: this.value,
      menu: null
    }
  },
  watch: {
    innerValue(newVal) {
      this.$emit("input", newVal);
    },
    value(newVal) {
      this.innerValue = newVal;

      let self = this
      setTimeout(() => self.$refs.field.validate(), 100)
    }
  },
  mounted() {
    this.$refs.field.validate()
  }
}
</script>