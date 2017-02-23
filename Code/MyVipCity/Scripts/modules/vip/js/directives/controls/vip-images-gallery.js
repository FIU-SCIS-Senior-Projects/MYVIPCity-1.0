define(['vip/js/vip', 'jquery', 'angular', 'dropzone', 'sortable'], function (vip, jQuery, angular, dropzone, sortable) {
	'use strict';

	dropzone.autoDiscover = false;

	vip.directive('vipImagesGallery', ['$compile', function ($compile) {
		return {
			restrict: 'ACE',

			require: 'ngModel',

			// new scope
			scope: true,

			link: function (scope, element, attrs, ngModelCtrl) {
				var listeners = [];

				var buildFilesArrayForLightGallery = function (modelArray) {
					var array = [];
					angular.forEach(modelArray, function (file) {
						array.push({
							src: 'api/Pictures/' + file.BinaryDataId
						});
					});
					return array;
				};

				var initializePreviewPictures = function() {
					if (!scope.previewPictures)
						scope.previewPictures = [];
					else {
						scope.previewPictures.length = 0;
					}
				};

				var buildGallery = function () {
					var galleryElement = angular.element(
						'<div class="vip-light-gallery list-unstyled justified-gallery">' +
							'<a href="#" target="_self" ng-repeat="pic in previewPictures">' +
								'<img src="{{pic.src}}" alt="">' +
							'</a>' +
						'</div>'
					);
					var existingGalleryElement = element.find('.vip-light-gallery');
					if (existingGalleryElement.length) {
						var lg = existingGalleryElement.data('lightGallery');
						if (lg)
							lg.destroy(true);
						jQuery(element).empty();
					}
					var files = buildFilesArrayForLightGallery(ngModelCtrl.$viewValue || []);
					initializePreviewPictures();
					// get first picture if any
					if (files && files.length)
						scope.previewPictures.push(files[0]);
					
					element.append(galleryElement);
					$compile(galleryElement)(scope);

					jQuery(galleryElement).click(function (e) {
						e.preventDefault();
						jQuery(element.find('.vip-light-gallery')).lightGallery({
							hash: false,
							galleryId: scope.$id,
							dynamic: true,
							dynamicEl: files,
							thumbnail: false
						});
					});
				};

				ngModelCtrl.$render = function () {
					buildGallery();
				};

				listeners.push(scope.$watch(attrs.ngModel, buildGallery));

				listeners.push(scope.$on('$destroy', function () {
					lg = null;
					// unregister listeners
					for (var i = 0; i < listeners.length; i++)
						listeners[i]();
				}));
			}
		};
	}]);
});