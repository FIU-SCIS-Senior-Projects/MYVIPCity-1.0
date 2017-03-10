define(['vip/js/vip', 'jquery', 'cropper', 'dropzone'], function (vip, jQuery) {
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
					'<button ng-show="!uploading" class="vip-rendering-edit btn btn-primary vip-profile-picture__change-img-btn">Change Picture</button>' + 
					'<div ng-show="uploading" class="vip-rendering-edit vip-profile-picture__uploading">' +
						'<span><i class="zmdi zmdi-refresh"></i>Uploading...</span>' +
					'</div>',

			link: function (scope, element, attrs, ngModelCtrl) {
				var listeners = [];

				var imgEl = element.find('img.vip-profile-picture__cropper-img');
				var imgPreviewEl = element.find('.vip-profile-picture__read-mode-container');
				var imgPreviewElXs = element.find('.vip-profile-picture__read-mode-container-xs');

				// set the cropper with two previews
				jQuery(imgEl).cropper({
					aspectRatio: 1 / 1,
					preview: [imgPreviewEl[0], imgPreviewElXs[0]],
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

				// set the dropzone
				element.find('.vip-profile-picture__change-img-btn').dropzone({
					url: 'api/Pictures',
					maxFileSize: 5,
					addRemoveLinks: true,
					dictDefaultMessage: 'Drop images here or click to upload',
					previewsContainer: null,
					createImageThumbnails: false,
					previewTemplate: '<div id="preview-template" style="display: none;"></div>',
					parallelUploads: 1,
					uploadMultiple: false,
					maxFiles: 1,
					init: function () {
						// success event handler
						this.on('success', function (file, response) {
							//// add the images to the array
							//scope.$apply(function () {
							//	// add the new files to the array
							//	scope.files.push.apply(scope.files, response);
							//});
						});
						// complete event handler
						this.on('complete', function (file) {
							// upload ended
							scope.$apply(function() {
								scope.uploading = false;
							});
							
							// remove the file from the dropzone once it completes
							this.removeFile(file);
						});
					},
					sending: function () {
						// upload started
						scope.$apply(function () {
							scope.uploading = true;
						});
					}
				});


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