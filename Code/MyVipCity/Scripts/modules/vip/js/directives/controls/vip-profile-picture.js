define(['vip/js/vip', 'jquery', 'angular', 'cropper'], function (vip, jQuery, angular) {
	'use strict';

	vip.directive('vipProfilePicture', [function () {
		return {
			restrict: 'ACE',

			require: 'ngModel',

			// new scope
			scope: true,

			template: '<img src="/Content/img/tempPics/therock.jpg"></img>',

			link: function (scope, element, attrs, ngModelCtrl) {
				var listeners = [];

				var imgEl = jQuery(element).find('img');
				jQuery(imgEl).cropper({
					aspectRatio: 1 / 1,
					guides: false,
					viewMode: 3,
					dragMode: 'move',
					modal: false,
					autoCrop: true,
					autoCropArea: 1,
					cropBoxMovable: false,
					cropBoxResizable: false,
					toggleDragModeOnDblclick: false,
					crop: function(e) {
						var x = 1;
					}
				});

				listeners.push(scope.$on('$destroy', function () {
					// unregister listeners
					for (var i = 0; i < listeners.length; i++)
						listeners[i]();
				}));
			}
		};
	}]);
});