import Vue from "vue";

import home from "./Home.vue";

let v = new Vue({
	el: "#home",
	components: {
		'home': home
	}
});
