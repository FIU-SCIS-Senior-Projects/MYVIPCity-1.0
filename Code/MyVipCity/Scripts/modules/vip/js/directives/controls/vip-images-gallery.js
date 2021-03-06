﻿define(['vip/js/vip', 'jquery', 'angular'], function (vip, jQuery, angular) {
	'use strict';

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
							src: 'api/Pictures/' + file.BinaryDataId,
							Index: file.Index
						});
					});
					array.sort(function(a, b) {
						return a.Index - b.Index;
					});
					return array;
				};

				var initializePreviewPicturesArray = function () {
					if (!scope.previewPictures)
						scope.previewPictures = [];
					else {
						scope.previewPictures.length = 0;
					}
				};

				var buildGallery = function () {
					// build gallery element
					var galleryElement = angular.element(
						'<div class="vip-light-gallery list-unstyled justified-gallery">' +
							'<img ng-src="{{pic.src}}" alt="" ng-repeat="pic in previewPictures | orderBy: \'Index\'"/>' +
							'<div class="vip-light-gallery__more-pics"ng-show="morePictures" title="More pictures in this gallery"><i class="zmdi zmdi-collection-image-o"></i></div>' +
						'</div>'
					);
					// search for a previously added gallery element inside this element
					var existingGalleryElement = element.find('.vip-light-gallery');
					// if there was a previous gallery
					if (existingGalleryElement.length) {
						// search the plugin
						var lg = existingGalleryElement.data('lightGallery');
						// destroy it
						if (lg)
							lg.destroy(true);
						// empty the element
						jQuery(element).empty();
					}
					// build the array lightGallery plugin understands
					var files = buildFilesArrayForLightGallery(ngModelCtrl.$viewValue || []);

					initializePreviewPicturesArray();
					// get first picture if any
					if (files && files.length) {
						scope.previewPictures.push(files[0]);
					}

					scope.morePictures = files && files.length > 1;

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

				listeners.push(scope.$on('$destroy', function () {
					// unregister listeners
					for (var i = 0; i < listeners.length; i++)
						listeners[i]();
				}));
			}
		};
	}]);
});