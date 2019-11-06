<template>
  <div>
    <ValidationProvider
      :rules="rules"
      v-slot="{ classes }"
    >
      <input
        class="form-control inline-edit"
        :class="classes"
        :type="fieldtype"
        ref="inputElem"
        v-model.lazy="input"
        :readonly="!edit"
        @blur="update"
        @keydown.enter="save"
      >
    </ValidationProvider>
  </div>
</template>

<script>
export default {
  name: 'InlineEdit',
  props: ['value', 'edit', 'valid', 'rules', 'fieldtype'],
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
      this.$emit('update', this.input);
    }, 
    save: () =>  {
      this.$emit('update', this.input);
      this.$emit('save')
    }
  }
}
</script>

<style scoped>
input.inline-edit {
  text-align: inherit;
  padding: 5px;
}
input.inline-edit:read-write {
  border-radius: 5px;
  border: 2px solid rgb(102, 187, 106);
}
input.inline-edit.invalid:read-write {
  border-color: rgb(255, 82, 82);
}
</style>