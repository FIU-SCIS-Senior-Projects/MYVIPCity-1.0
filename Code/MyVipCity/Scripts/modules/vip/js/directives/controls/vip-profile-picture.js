define(['vip/js/vip', 'jquery', 'angular', 'cropper'], function (vip, jQuery, angular) {
	'use strict';

	vip.directive('vipProfilePicture', ['$compile', function ($compile) {
		return {
			restrict: 'ACE',

			require: 'ngModel',

			// new scope
			scope: true,

			template:
					'<div class="vip-rendering-read vip-profile-picture__read-mode-container"></div>' +
					'<div class="vip-rendering-read vip-profile-picture__read-mode-container-xs"></div>' +
					'<div class="vip-rendering-edit vip-profile-picture__edit-mode-container">' +
						'<img class="vip-profile-picture__cropper-img" src="/Content/img/tempPics/therock.jpg">' +
					'</div>' +
					'<button class="vip-rendering-edit btn btn-primary vip-profile-picture__change-img-btn">Change Picture</button>',

			link: function (scope, element, attrs, ngModelCtrl) {
				var listeners = [];

				var imgEl = element.find('img.vip-profile-picture__cropper-img');
				var imgPreviewEl = element.find('.vip-profile-picture__read-mode-container');
				var imgPreviewEl2 = element.find('.vip-profile-picture__read-mode-container-xs');

				jQuery(imgEl).cropper({
					aspectRatio: 1 / 1,
					preview: [imgPreviewEl[0], imgPreviewEl2[0]],
					viewMode: 3,
					dragMode: 'move',
					autoCrop: true,
					autoCropArea: 1,
					toggleDragModeOnDblclick: false,
					cropBoxResizable: true,
					cropBoxMovable: true,
					restore: false,
					center: false,
					guides: false,
					highlight: false,
					responsive: false
				});
				var cropper = jQuery(imgEl).data('cropper');

				listeners.push(scope.$watch('renderingMode', function (renderingMode) {
					if (renderingMode === vip.renderingModes.edit) {
						element.find('.vip-rendering-read').hide();
						element.find('.vip-rendering-edit').show();
					}
					else {
						element.find('.vip-rendering-read').show();
						element.find('.vip-rendering-edit').hide();
					}
				}));

				listeners.push(scope.$on('$destroy', function () {
					// unregister listeners
					for (var i = 0; i < listeners.length; i++)
						listeners[i]();
				}));
			}
		};
	}]);
});