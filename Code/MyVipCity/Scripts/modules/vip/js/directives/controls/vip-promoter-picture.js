define(['vip/js/vip', 'jquery', 'angular'], function(vip, jQuery, angular) {
	'use strict';

	vip.directive('vipPromoterPicture', [
		'$compile', '$timeout', function($compile) {
			return {
				require: 'ngModel',

				scope: true,

				link: function(scope, element, attrs, ngModelCtrl) {
					ngModelCtrl.$render = function() {
						scope.promoters = ngModelCtrl.$modelValue;
					};
					var width = attrs.width ? parseInt(attrs.width) : 70;
					var height = attrs.height ? parseInt(attrs.height) : 70;
					var borderRadius = attrs.borderRadius || '50%';

					element.attr('style', 'width:' + width + 'px !important;height:' + height + 'px !important;border-radius:' + borderRadius + ';overflow:hidden !important');

					// get style for the image
					scope.getStyle = function(promoter) {
						if (!promoter.ProfilePicture)
							return "";
						// container W and H can me made dynamic and configurable
						var containerWidth = width, containerHeight = height;
						var cropData = JSON.parse(promoter.ProfilePicture.CropData);
						// check if the picture has a rotation of (2x+1)*90 degrees
						if (cropData.rotate && (cropData.rotate / 90) % 2 === 1) {
							var tmp = cropData.x;
							cropData.x = cropData.y;
							cropData.y = tmp;
						}
						// get ration between destination container and crop dimensions
						var rw = (containerWidth * 1.0) / cropData.width;
						var rh = (containerHeight * 1.0) / cropData.height;
						// create style dictionary
						var style = {};
						// set styles
						style.width = cropData.naturalWidth * rw + 'px';
						style.height = cropData.naturalHeight * rw + 'px';
						style.transform = 'translateX(' + (-cropData.x * rw) + 'px) translateY(' + (-cropData.y * rh) + 'px) rotate(' + (cropData.rotate || 0) + 'deg)';
						// convert style to string
						var result = '';
						angular.forEach(style, function(value, key) {
							if (style.hasOwnProperty(key)) {
								result += key + ':' + value + ' !important;';
							}
						});

						return result;
					};

					var imgElement = angular.element('<img alt="" ng-src="api/Pictures/{{promoter.ProfilePicture.BinaryDataId}}" style="{{::getStyle(promoter)}}">');
					if (attrs.imgClass) {
						imgElement.addClass(attrs.imgClass);
						element.removeAttr('img-class');
					}
					element.append(imgElement);
					$compile(imgElement)(scope);

				}
			};
		}
	]);
});