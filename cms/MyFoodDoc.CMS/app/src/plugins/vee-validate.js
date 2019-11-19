import Vue from 'vue';
import { ValidationProvider } from 'vee-validate';

import { extend } from 'vee-validate';
import { required, email, numeric, max } from 'vee-validate/dist/rules';

extend('required', required);
extend('email', email);
extend('numeric', numeric);
extend('max', max);

// Register it globally
Vue.component('ValidationProvider', ValidationProvider);