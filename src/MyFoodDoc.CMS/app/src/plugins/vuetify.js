import Vue from "vue";
import Vuetify from "vuetify";
import theme from "./theme";
import { TiptapVuetifyPlugin } from 'tiptap-vuetify'
import VExtensions from "@/components/extensions"
import 'tiptap-vuetify/dist/main.css'
import "vuetify/dist/vuetify.min.css";
import "@mdi/font/css/materialdesignicons.css";
import VuetifyConfirm from 'vuetify-confirm'

Vue.use(Vuetify);

const vuetify = new Vuetify({
  iconfont: "mdi",
  theme
})

Vue.use(TiptapVuetifyPlugin, {
  // the next line is important! You need to provide the Vuetify Object to this place.
  vuetify, // same as "vuetify: vuetify"
  // optional, default to 'md' (default vuetify icons before v2.0.0)
  iconsGroup: 'mdi'
})

Vue.use(VExtensions, { vuetify } )

Vue.use(VuetifyConfirm, { vuetify })

export default vuetify;
