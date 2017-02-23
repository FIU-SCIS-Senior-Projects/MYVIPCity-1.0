define(['vip/js/vip', 'jquery', 'angular', 'dropzone'], function (vip, jQuery, angular, dropzone) {
	'use strict';

	dropzone.autoDiscover = false;

	vip.directive('vipImagesEdit', [function () {
		return {
			restrict: 'ACE',

			require: 'ngModel',

			// new scope
			scope: true,

			template:
				'<div>' +
					'<div class="vip-image-preview">' +
						'<ul class="clearfix vip-image-preview__img-list">' +
							'<li ng-repeat="pic in files">' +
								 '<img class="vip-image-preview__img animated bounceIn" src="api/Pictures/{{pic.BinaryDataId}}" alt="{{pic.FileName}}"/>' +
								 //'<div>Remove</div>' +
							'</li>' +
						'</ul>' +
					'</div>' +
					'<div class="vip-dropzone-container dropzone"></div>' +
				'</div>',

			controller: ['$scope', function ($scope) {
				$scope.files = [];
			}],
			link: function (scope, element, attrs) {
				var listeners = [];

				// TODO: Remove this
				jQuery('.light-gallery').lightGallery({
					hash: false,
					// galleryId: scope.$id,
					thumbnail: true
				});

				var dropzoneElement = element.find('.vip-dropzone-container');
				jQuery(dropzoneElement).dropzone({
					url: 'api/Pictures',
					maxFileSize: 5,
					addRemoveLinks: true,
					init: function () {
						// success event handler
						this.on('success', function (file, response) {
							scope.$apply(function () {
								scope.files.push.apply(scope.files, response);
							});
						});
						// complete event handler
						this.on('complete', function (file) {
							this.removeFile(file);
						});
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