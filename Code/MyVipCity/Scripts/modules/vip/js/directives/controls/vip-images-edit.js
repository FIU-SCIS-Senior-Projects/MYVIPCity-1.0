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
								'<div class="vip-image-preview__remove-img"><i class="zmdi zmdi-close" ng-click="removePicture($event, pic)"></i></div>' +
								 '<img class="vip-image-preview__img" src="api/Pictures/{{pic.BinaryDataId}}" alt="{{pic.FileName}}"/>' +
							'</li>' +
						'</ul>' +
					'</div>' +
					'<div class="vip-dropzone-container dropzone"></div>' +
				'</div>',

			controller: ['$scope', function ($scope) {
				$scope.files = [];
			}],
			link: function (scope, element, attrs, ngModelCtrl) {
				var listeners = [];

				ngModelCtrl.$render = function() {
					scope.files = ngModelCtrl.$viewValue || [];
				}

				// TODO COmment code
				// TODO: Remove this
				jQuery('.light-gallery').lightGallery({
					hash: false,
					// galleryId: scope.$id,
					thumbnail: true
				});

				// find the element on which the dropzone jquery plugin will be initialized
				var dropzoneElement = element.find('.vip-dropzone-container');
				// initialize dropzone
				jQuery(dropzoneElement).dropzone({
					url: 'api/Pictures',
					maxFileSize: 5,
					addRemoveLinks: true,
					dictDefaultMessage: 'Drop images here or click to upload',
					init: function () {
						// success event handler
						this.on('success', function (file, response) {
							// add the images to the array
							scope.$apply(function () {
								// add the new files to the array
								scope.files.push.apply(scope.files, response);
								// update ngModel
								ngModelCtrl.$setViewValue(scope.files);
							});
							// wait 1 sec and remove the animated class this is to avoid the animation from being repeated when sorting the images)
							$timeout(function () {
								var animatedImages = element.find('.animated');
								animatedImages.removeClass('animated');
							}, 1000, false);
						});
						// complete event handler
						this.on('complete', function (file) {
							// remove the file from the dropzone once it completes
							this.removeFile(file);
						});
					}
				});

				// find the element where sortable plugin will be initialized
				var list = element.find('.vip-image-preview__img-list');
				// initialize sortable
				sortable.create(list[0], {
					// add a handler for update event
					onUpdate: function (e) {
						// get the new index of the element
						var newIndex = e.newIndex;
						// get the olde index of the element
						var oldIndex = e.oldIndex;
						// re-arrange the files array
						if (oldIndex < newIndex) {
							scope.files.splice(newIndex + 1, 0, scope.files[oldIndex]);
							scope.files.splice(oldIndex, 1);
						}
						else {
							scope.files.splice(newIndex, 0, scope.files[oldIndex]);
							scope.files.splice(oldIndex + 1, 1);
						}
						// update ngModel
						ngModelCtrl.$setViewValue(scope.files);
					}
				});

				// handles remove picture
				scope.removePicture = function ($event, picture) {
					// find the ancestor li
					var li = jQuery($event.currentTarget).closest('li');
					// add an exit animation
					li.addClass("animated zoomOut");
					// find the index in th array of the picture being deleted
					var index = scope.files.indexOf(picture);
					// remove it
					if (index > -1) {
						scope.files.splice(index, 1);
						// update ngModel
						ngModelCtrl.$setViewValue(scope.files);
					}
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