import '../scss/theme.scss';

import $ from 'jquery';
window.$ = window.jQuery = $;
import 'popper.js';
import 'bootstrap';
require('bootstrap-filestyle2');
require('datatables.net-bs4')(window, $);
import 'datatables.net-responsive';
import 'datatables.net-responsive-bs4';
import dayjs from 'dayjs';
window.dayjs = dayjs;
import 'jquery-validation';
import 'jquery-validation-unobtrusive';
require('select2')($);
import Stickyfill from 'stickyfilljs';
window.Stickyfill = Stickyfill;
import Vue from 'vue';
import BootstrapVue from 'bootstrap-vue';
window.Vue = Vue;
import VueSlider from 'vue-slider-component';
window.VueSlider = VueSlider;
import axios from 'axios';
window.axios = axios;
var _ = require('lodash');
window._ = _;

Vue.use(BootstrapVue);