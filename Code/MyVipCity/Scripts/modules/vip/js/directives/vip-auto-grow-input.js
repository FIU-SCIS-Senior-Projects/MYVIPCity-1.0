define(['vip/js/vip', 'jquery', 'jquery-auto-grow-input'], function (vip, jQuery) {
	'use strict';

	vip.directive('vipAutoGrowInput', ['$timeout', function ($timeout) {
		return {
			restrict: 'AC',
			priority: 1000,
			link: function (scope, element, attrs) {
				// set the max width
				var maxWidth = attrs.autoGrowMax || 1000;
				// enable auto grow jquery plugin
				jQuery(element).autoGrowInput();
				// trigger autogrow
				$timeout(function () {
					jQuery(element).trigger('autogrow');
				}, 0, false);
			}
		};
	}]);
});