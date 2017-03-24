define(['vip/js/vip'], function (vip) {
	'use strict';

	vip.directive('vipVideo', ['vipControlRenderingService', 'vipVideoService', '$compile', function (vipControlRenderingService, vipVideoService, $compile) {
		return {
			restrict: 'ACE',

			require: 'ngModel',

			// new scope
			scope: true,

			link: function (scope, element, attrs, ngModelCtrl) {

				var innerScope;

				// destroys the scope used to compile the video element
				var destroyInnerScope = function () {
					if (innerScope) {
						innerScope.$destroy();
						innerScope = null;
						element.empty();
					}
				};

				var showVideo = function() {
					var videoElement = vipVideoService.getVideoElement(ngModelCtrl.$modelValue);
					if (videoElement) {
						element.append(videoElement);
						innerScope = scope.$new();
						$compile(videoElement)(innerScope);
					}
				};

				ngModelCtrl.$render = function () {
					destroyInnerScope();
					showVideo();
				};
			}
		};
	}]);
});