define(['vip/js/vip', 'jquery', 'angular'], function (vip, jQuery, angular) {
	'use strict';

	vip.directive('vipExistingPromoters', ['$http', function ($http) {
		return {
			restrict: 'ACE',

			// new scope
			scope: true,

			template:
				'<a ng-repeat="promoter in promoters" class="list-group-item media" href="#/promoter-profile/{{promoter.Id}}">' +
                    '<div class="pull-left" style="width:70px !important; height: 70px !important; overflow: hidden !important; border-radius:50% !important;">' +
                        '<img alt="" class="list-group__img img-circle vip-existing-promoters__img" ng-src="api/Pictures/{{promoter.ProfilePicture.BinaryDataId}}" style="{{::getStyle(promoter, $element)}}">' +
                    '</div>' +
                    '<div class="media-body list-group__text">' +
						'<div vip-promoter-name ng-model="promoter" wrap-with="strong" vip-read-only></div>' +
						'<span class="rmd-rate" vip-rating ng-model="promoter.AverageRating" read-only="true"></span>' +
						'<small ng-show="renderingMode == 2" class="list-group__text vip-existing-promoter__email">{{promoter.Email}}<small/>' +
                    '</div>' +
                '</a>',

			link: function (scope, element, attrs) {
				var listeners = [];

				// get style for the image
				scope.getStyle = function (promoter) {
					if (!promoter.ProfilePicture)
						return "";
					// container W and H can me made dynamic and configurable
					var containerWidth = 70, containerHeight = 70;
					var cropData = JSON.parse(promoter.ProfilePicture.CropData);
					// get ration between destination container and crop dimensions
					var rw = (containerWidth * 1.0) / cropData.width;
					var rh = (containerHeight * 1.0) / cropData.height;
					// create style dictionary
					var style = {};
					// set styles
					style.width = cropData.naturalWidth * rw + 'px';
					style.height = cropData.naturalHeight * rw + 'px';
					style.transform = 'translateX(' + (-cropData.x * rw) + 'px) translateY(' + (-cropData.y * rh) + 'px)';
					// convert style to string
					var result = '';
					angular.forEach(style, function (value, key) {
						if (style.hasOwnProperty(key)) {
							result += key + ':' + value + ' !important;';
						}
					});

					return result;
				};

				listeners.push(attrs.$observe('vipBusinessId', function (idValue) {
					// get the attribute value as an int
					var id = parseInt(idValue);
					// make sure is a valid int
					if (!isNaN(id) && id) {
						// make a request to bring all the promoters associated to this business
						$http.get('/api/Business/' + id + '/Promoters').then(function (response) {
							scope.promoters = response.data;
							// TODO: Get actual average rating
							angular.forEach(scope.promoters, function (promoter) {
								promoter.AverageRating = 5;
							});

							// watch for changes in rendering mode
							var unregister = scope.$watch('renderingMode', function (value) {
								// check edit
								if (value === vip.renderingModes.edit) {
									// unregister the listener
									unregister();
									// loop all promoters and make a request to find the email
									angular.forEach(scope.promoters, function (promoter) {
										$http.get('/api/PromoterProfile/' + promoter.Id + '/Email').then(function (response) {
											promoter.Email = response.data;
										});
									});
								}
							});
						}, function (error) {
						});
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