import Vue from 'vue';
window.Vue = Vue;
var vueMixins = [];
window.vueMixins = vueMixins;

import moment from 'vue-moment'
Vue.use(moment);

import VueSweetalert2 from 'vue-sweetalert2';
Vue.use(VueSweetalert2);

import vueCustomElement from 'vue-custom-element';
Vue.use(vueCustomElement);

import draggable from 'vuedraggable';
window.draggable = draggable;

import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'

import { BootstrapVue, IconsPlugin } from 'bootstrap-vue';
Vue.use(BootstrapVue);
Vue.use(IconsPlugin);

import axios from 'axios';
window.axios = axios;

import kanbanComponent from './components/Kanban.vue';
Vue.customElement('kanban-board', kanbanComponent); 