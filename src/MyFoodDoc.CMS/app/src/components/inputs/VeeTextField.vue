<template>
    <ValidationProvider ref="field"
                        v-slot="{ errors, valid }"
                        :name="$attrs.label"
                        :rules="rules"
                        style="display: contents;">
        <v-text-field v-model="innerValue"
                      :error-messages="errors"
                      :success="valid"
                      v-bind="$attrs"
                      v-on="$listeners" />
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
            },
            number: {
                type: Boolean,
                default: false
            }
        },
        data() {
            return {
                innerValue: this.value
            }
        },
        watch: {
            innerValue(newVal) {
                this.$emit("input", (newVal == null || newVal == '') ? null : this.number ? this.parseNumber(newVal) : newVal);
            },
            value(newVal) {
                this.innerValue = newVal;

                let self = this
                setTimeout(() => self.$refs.field.validate(), 10)
            }
        },
        mounted() {
            this.$refs.field.validate()
        },
        methods: {
            parseNumber(x) {
                const parsed = Number.parseFloat(x);

                if (Number.isNaN(parsed)) {
                    return null;
                }

                return parsed;
            }
        }
    };
</script>
