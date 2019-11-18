import VTipTapArea from './VTipTapArea.vue'
import VImage from './VImage'

const VElements = {
  install (Vue) {
    Vue.component('v-tiptap-area', VTipTapArea)
    Vue.component('v-image', VImage)
  },
}

export { VTipTapArea }
export default VElements