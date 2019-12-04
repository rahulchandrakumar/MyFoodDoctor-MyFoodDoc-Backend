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
          :value="innerValue == null ? null : $moment(innerValue).format(displayDateFormat)"
          :error-messages="errors"
          :success="valid"
          v-bind="$attrs"
          prepend-icon="mdi-calendar"
          readonly
          v-on="on"
        />
      </ValidationProvider>
    </template>
    <v-date-picker v-model="pickerValue" @input="menu = false" />
  </v-menu>
</template>

<script>
import { displayDateFormat, pickerDateFormat } from "@/utils/Consts.js"

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
      pickerValue: this.value == null ? null : this.$moment(this.value).format(pickerDateFormat),
      menu: null,
      displayDateFormat: displayDateFormat
    }
  },
  watch: {
    pickerValue(newVal) {
      this.$emit("input", newVal == null ? null : this.$moment(newVal, pickerDateFormat).toISOString());
    },
    innerValue(newVal) {
      this.$emit("input", newVal);
    },
    value(newVal) {
      this.innerValue = newVal;
      this.pickerValue = newVal == null ? null : this.$moment(newVal).format(pickerDateFormat)

      let self = this
      setTimeout(() => self.$refs.field.validate(), 100)
    }
  },
  mounted() {
    this.$refs.field.validate()
  }
}
</script>