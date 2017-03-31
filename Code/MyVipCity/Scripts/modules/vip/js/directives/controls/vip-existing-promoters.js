define(['vip/js/vip', 'jquery', 'angular', 'sweet-alert', 'lodash'], function (vip, jQuery, angular, swal, _) {
	'use strict';

	vip.directive('vipExistingPromoters', ['$http', function ($http) {
		return {
			restrict: 'ACE',

			// new scope
			scope: true,

			template:
				'<div ng-repeat="promoter in promoters" >' +
					'<i ng-show="renderingMode == 2" class="zmdi zmdi-close vip-existing-promoters__delete" ng-click="removePromoter($event, promoter)"></i>' +

					'<a class="list-group-item media" href="#/promoter-profile/{{promoter.Id}}">' +
						'<div vip-promoter-picture ng-model="promoter" class="vip-existing-promoters__img-container pull-left" img-class="list-group__img img-circle vip-existing-promoters__img">' +
						'</div>' +
						'<div class="media-body">' +
							'<div vip-promoter-name ng-model="promoter" wrap-with="strong" vip-read-only></div>' +
							'<span class="rmd-rate" vip-rating ng-model="promoter.AverageRating" read-only="true"></span>' +
							'<small ng-show="renderingMode == 2" class="list-group__text vip-existing-promoter__email">{{promoter.Email}}<small/>' +
						'</div>' +
					'</a>' +
				'</div>',

			link: function (scope, element, attrs) {
				var listeners = [];

				// get style for the image
				scope.getStyle = function (promoter) {
					if (!promoter.ProfilePicture)
						return "";
					// container W and H can me made dynamic and configurable
					var containerWidth = 70, containerHeight = 70;
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
					angular.forEach(style, function (value, key) {
						if (style.hasOwnProperty(key)) {
							result += key + ':' + value + ' !important;';
						}
					});

					return result;
				};

				var showErrorPopup = function (title, errorMsg) {
					swal(title || 'Oops', errorMsg || 'An error has occurred', 'error');
				};

				scope.removePromoter = function ($event, promoter) {
					swal({
						type: 'warning',
						title: 'Delete Promoter?',
						text: 'Are you sure you want to delete this promoter profile? This action cannot be rolled back.',
						confirmButtonText: "Yes, delete it",
						showCancelButton: true
					}).then(function () {
						$http.delete('api/PromoterProfile/' + promoter.Id).then(function () {
							_.remove(scope.promoters, function (p) {
								return p.Id === promoter.Id;
							});
						}, function () {
							showErrorPopup();
						});
					}, angular.noop);
				};

				listeners.push(attrs.$observe('vipBusinessId', function (idValue) {
					// get the attribute value as an int
					var id = parseInt(idValue);
					// make sure is a valid int
					if (!isNaN(id) && id) {
						// make a request to bring all the promoters associated to this business
						$http.get('/api/Business/' + id + '/Promoters').then(function (response) {
							scope.promoters = response.data;

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
