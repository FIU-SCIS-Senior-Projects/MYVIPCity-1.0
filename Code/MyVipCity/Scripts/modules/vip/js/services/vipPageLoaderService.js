define(['vip/js/vip'], function (vip) {
	'use strict';

	vip.factory('vipPageLoaderService', [function () {

		return {
			showPageLoader: function() {
				var element = document.getElementById('page-loader');
				var op = 0.7;
				element.style.opacity = op;
				element.style.filter = 'alpha(opacity=' + op * 100 + ')';
				element.style.display = 'block';
			},
			hidePageLoader: function () {
				var element = document.getElementById('page-loader');
				element.style.display = 'none';
			}
		};
	}]);
});