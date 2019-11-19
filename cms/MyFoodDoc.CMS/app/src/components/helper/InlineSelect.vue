<template>
  <div>
    <span>
        <v-select
          class="inline-select"
          :items="items"
          :readonly="!edit"
          v-model.lazy="input"
          @blur="update"
        ></v-select>
    </span>
  </div>
</template>

<script>
export default {
  name: 'InlineSelect',
  props: ['value', 'edit', 'items'],
  data() {
    return {
      input: this.value
    }
  },
  watch: {
    value: function (newVal) {
      this.input = newVal;
    }
  },
  methods: {
    update: function () {
      this.$emit('update:value', this.input);
    }
  }
}
</script>

<style>
.v-text-field.inline-select {
  padding-top: 18px;
}
.inline-select .v-select__slot {
  text-align: inherit;
  padding: 4px;
}
.inline-select:not(.v-input--is-readonly) .v-select__slot {
  border-radius: 5px;
  border: 2px solid rgb(102, 187, 106);
}
</style>