define(['vip/js/vip', 'jquery', 'angular', 'dropzone', 'sortable'], function (vip, jQuery, angular, dropzone, sortable) {
	'use strict';

	dropzone.autoDiscover = false;

	vip.directive('vipImagesEdit', ['$timeout', function ($timeout) {
		return {
			restrict: 'ACE',

			require: 'ngModel',

			// new scope
			scope: true,

			template:
				'<div>' +
					'<div class="vip-image-preview">' +
						'<ul class="vip-image-preview__img-list">' +
							'<li ng-repeat="pic in files" class="id_{{pic.BinaryDataId}} animated bounceIn">' +
								'<div class="vip-image-preview__remove-img"><i class="zmdi zmdi-close"></i></div>' +
								 '<img class="vip-image-preview__img" src="api/Pictures/{{pic.BinaryDataId}}" alt="{{pic.FileName}}"/>' +
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

				// TODO COmment code
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
							$timeout(function() {
								var animatedImages = element.find('img.animated');
								animatedImages.removeClass('animated');
							}, 1000, false);
						});
						// complete event handler
						this.on('complete', function (file) {
							this.removeFile(file);
						});
					}
				});

				var list = element.find('.vip-image-preview__img-list');
				sortable.create(list[0], {
					onUpdate: function (e) {
						var newIndex = e.newIndex;
						var oldIndex = e.oldIndex;
						if (oldIndex < newIndex) {
							scope.files.splice(newIndex + 1, 0, scope.files[oldIndex]);
							scope.files.splice(oldIndex, 1);
						}
						else {
							scope.files.splice(newIndex, 0, scope.files[oldIndex]);
							scope.files.splice(oldIndex + 1, 1);
						}
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