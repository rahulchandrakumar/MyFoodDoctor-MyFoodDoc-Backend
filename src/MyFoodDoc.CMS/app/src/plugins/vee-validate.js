import Vue from "vue";
import { ValidationProvider, ValidationObserver } from "vee-validate";

import { extend } from "vee-validate";
import { required, email, numeric, integer, max, min, min_value } from "vee-validate/dist/rules";
import { setInteractionMode } from 'vee-validate';

import moment from 'moment';

import { displayDateFormat } from "@/utils/Consts.js"

extend("required", required);
extend("email", email);
extend("numeric", numeric);
extend("integer", integer);
extend("max", max);
extend("min", min);
extend("min_value", min_value);
extend("dateLess", {
  params: ['date'],
  validate(value, { date }) {
    return moment(value, displayDateFormat) < moment(date);
  }
});
extend("dateMore", {
  params: ['date'],
  validate(value, { date }) {
    return moment(value, displayDateFormat) > moment(date);
  }
});
extend("decimal", {
  validate: value => /^\d+\.?\d{0,3}$/.test(value)
});

// Register it globally
Vue.component("ValidationProvider", ValidationProvider);
Vue.component("ValidationObserver", ValidationObserver);
setInteractionMode('eager');