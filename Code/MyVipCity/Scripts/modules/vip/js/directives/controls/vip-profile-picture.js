define(['vip/js/vip', 'jquery', 'angular', 'cropper', 'dropzone'], function (vip, jQuery, angular) {
	'use strict';

	vip.directive('vipProfilePicture', [function () {
		return {
			restrict: 'ACE',

			require: 'ngModel',

			// new scope
			scope: true,

			template:
					'<div class="vip-rendering-read vip-profile-picture__read-mode-container vip-rm-{{renderingMode}}"></div>' +
					'<div class="vip-rendering-read vip-profile-picture__read-mode-container-xs vip-rm-{{renderingMode}}"></div>' +
					'<div class="vip-rendering-edit vip-profile-picture__edit-mode-container vip-rm-{{renderingMode}}">' +
						'<img class="vip-profile-picture__cropper-img">' +
					'</div>' +
					'<button ng-show="!uploading" class="vip-rendering-edit btn btn-primary vip-profile-picture__change-img-btn vip-rm-{{renderingMode}}">Change Picture</button>' +
					'<div ng-show="uploading" class="vip-rendering-edit vip-profile-picture__uploading vip-rm-{{renderingMode}}">' +
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
					responsive: false,
					ready: function (e) {
						// get the cropper instance
						var cropperInstance = jQuery(e.target).data('cropper');
						// get the value from ngModelCtrl
						var value = ngModelCtrl.$viewValue;
						// check if there is a crop data
						if (value && value.CropData) {
							// apply the crop data
							var data = JSON.parse(value.CropData);
							cropperInstance.setData(data);
						}
					},
					crop: function (e) {
						var data = jQuery(e.target).data().cropper.getData();
						scope.$apply(function () {
							var newValue = angular.extend(ngModelCtrl.$viewValue || {}, { CropData: JSON.stringify(data) });
							ngModelCtrl.$setViewValue(newValue);
						});
					}
				});

				// get cropper instance
				var cropper = jQuery(imgEl).data('cropper');

				// set the picture for the cropper
				var setProfilePicture = function (profilePicture) {
					cropper.replace('api/Pictures/' + profilePicture.Picture.BinaryDataId);
				};

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
							scope.$apply(function () {
								var newValue = { Picture: response[0], CropData: null };
								ngModelCtrl.$setViewValue(newValue);
								setProfilePicture(newValue);
							});
						});
						// complete event handler
						this.on('complete', function (file) {
							// upload ended
							scope.$apply(function () {
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

				ngModelCtrl.$render = function () {
					var value = ngModelCtrl.$viewValue;
					if (value) {
						setProfilePicture(value);
					}
				};

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