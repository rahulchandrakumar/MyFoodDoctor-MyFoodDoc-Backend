import Vue from "vue";
import { ValidationProvider, ValidationObserver } from "vee-validate";

import { extend } from "vee-validate";
import { required, email, numeric, max, min } from "vee-validate/dist/rules";
import { setInteractionMode } from 'vee-validate';

extend("required", required);
extend("email", email);
extend("numeric", numeric);
extend("max", max);
extend("min", min);
extend("decimal", {
  validate: value => /^\d+\.?\d{0,3}$/.test(value)
});

// Register it globally
Vue.component("ValidationProvider", ValidationProvider);
Vue.component("ValidationObserver", ValidationObserver);
setInteractionMode('eager');