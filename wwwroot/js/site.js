//import "../scss/site.scss";

//axios.defaults.headers.common['RequestVerificationToken'] = document.querySelector('antiForgeryForm, input').getAttribute('value');

//var vueMixins = [];
//window.vueMixins = vueMixins;

import Vue from 'vue';
import vueCustomElement from 'vue-custom-element'
import helloWorld from 'wwwroot/js/components/helloworld'

Vue.use(vueCustomElement);

Vue.customElement('hello-world', helloWorld);
