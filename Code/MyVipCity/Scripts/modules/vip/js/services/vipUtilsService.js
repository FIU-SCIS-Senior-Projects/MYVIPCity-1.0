define(['vip/js/vip'], function (vip) {
	'use strict';

	vip.factory('vipUtilsService', [function () {

		return {
			getRandomIntInclusive: function (min, max) {
				min = Math.ceil(min);
				max = Math.floor(max);
				return Math.floor(Math.random() * (max - min + 1)) + min;
			},

			swap: function (array, i, j) {
				var tmp = array[i];
				array[i] = array[j];
				array[j] = tmp;
			},

			shuffleArray: function (array) {
				for (var i = 0; i < array.length; i++) {
					var j = this.getRandomIntInclusive(0, array.length - 1);
					this.swap(array, i, j);
				}
			},

			getRandomFromArray: function (array) {
				return array[this.getRandomIntInclusive(0, array.length - 1)];
			}
		};
	}]);
});