define(['vip/js/vip', 'jquery', 'angular', 'googleMaps', 'lodash'], function (vip, jQuery, angular, google, _) {
	'use strict';

	vip.directive('vipAddress', ['vipControlRenderingService', '$parse', '$http', 'vipApiKeys', '$timeout', '$compile', function (vipControlRenderingService, $parse, $http, vipApiKeys, $timeout, $compile) {
		return {
			restrict: 'ACE',

			require: 'ngModel',

			// new scope
			scope: true,

			link: function (scope, element, attrs, ngModelCtrl) {
				var listeners = [];
				var lastZipCode, lastStreet, lastCity, lastState, mapTimeout;

				scope._address = {};

				var clearMapAndHideElement = function() {
					jQuery(element.find('.vip-map')).empty().hide();
				};

				var clearMapAndShowElement = function () {
					jQuery(element.find('.vip-map')).empty().show();
				};

				ngModelCtrl.$render = function () {
					clearMapAndHideElement();
					if (ngModelCtrl.$viewValue) {
						scope._address.Street = ngModelCtrl.$viewValue.Street;
						scope._address.City = ngModelCtrl.$viewValue.City;
						scope._address.State = ngModelCtrl.$viewValue.State;
						scope._address.ZipCode = ngModelCtrl.$viewValue.ZipCode;
					}
				};

				var getAddressComponentByType = function (googResponse, type) {
					var addressComponents = googResponse.address_components;
					var component =  _.find(addressComponents, function (addComp) {
						return _.indexOf(addComp.types, type) > -1;
					});
					if (!component || !component.short_name)
						return '';
					return component.short_name;
				};

				var getStreetFromGoogleResponse = function (googResponse) {
					var streetNumberComponent = getAddressComponentByType(googResponse, 'street_number');
					var routeComponent = getAddressComponentByType(googResponse, 'route');

					return streetNumberComponent + ' ' + routeComponent;
				};

				var getCityFromGoogleResponse = function (googResponse) {
					var cityComponent = getAddressComponentByType(googResponse, 'locality');
					return cityComponent;
				};

				var getStateFromGoogleResponse = function (googResponse) {
					var stateComponent = getAddressComponentByType(googResponse, 'administrative_area_level_1');
					return stateComponent;
				};

				var getZipCodeFromGoogleResponse = function (googResponse) {
					var zipCodeComponent = getAddressComponentByType(googResponse, 'postal_code');
					return zipCodeComponent;
				};

				var getCountryFromGoogleResponse = function (googResponse) {
					var zipCodeComponent = getAddressComponentByType(googResponse, 'country');
					return zipCodeComponent;
				};

				var setNgModelValueFromGoogleResponse = function (googResponse) {
					var modelValue = {
						Street: getStreetFromGoogleResponse(googResponse),
						City: getCityFromGoogleResponse(googResponse),
						State: getStateFromGoogleResponse(googResponse),
						ZipCode: getZipCodeFromGoogleResponse(googResponse),
						Country: getCountryFromGoogleResponse(googResponse),
						Longitude: googResponse.geometry.location.lng,
						Latitude: googResponse.geometry.location.lat,
						FormattedAddress: googResponse.formatted_address
					};
					ngModelCtrl.$setViewValue(modelValue);
				};

				var processMap = function (fullAddress) {
					// cancel any previous map processing request
					if (mapTimeout)
						$timeout.cancel(mapTimeout);
					mapTimeout = $timeout(function () {
						// make a request to google geocoding api to get the latitude and longitude of the address
						$http.get('https://maps.googleapis.com/maps/api/geocode/json?address=' + fullAddress + '&key=' + vipApiKeys.googleGeocoding).then(function (response) {
							// get response data
							var data = response.data;
							// check for successful response
							if (data.status === 'OK') {
								// get the coordinates (longitude and latitude)
								var location = data.results[0].geometry.location;
								// create a map
								clearMapAndShowElement();
								var map = new google.maps.Map(element.find('.vip-map')[0], {
									zoom: 13,
									center: location
								});
								// create a marker with the location
								var marker = new google.maps.Marker({
									position: location,
									map: map
								});
								setNgModelValueFromGoogleResponse(data.results[0]);
							}
						}, function (error) {
							var modelValue = {
								Street: lastStreet,
								City: lastCity,
								State: lastState,
								ZipCode: lastZipCode
							};
							ngModelCtrl.$setViewValue(modelValue);
						});
					}, 1000, false);
				};

				var processAddress = function () {
					var street = scope._address.Street;
					var city = scope._address.City;
					var state = scope._address.State;
					var zipCode = scope._address.ZipCode;
					// make sure all fields are provided and different from the last set of values
					if (street && city && state && zipCode && (street !== lastStreet || city !== lastCity || state !== lastState || zipCode !== lastZipCode)) {
						// update previous set of address values
						lastStreet = street;
						lastCity = city;
						lastState = state;
						lastZipCode = zipCode;
						// get the full address
						var fullAddress = street + ', ' + city + ', ' + state + ' ' + zipCode;
						// process the map
						processMap(fullAddress);
					}
				};

				// create edit mode element
				var editElement = angular.element(
					'<div ng-show="renderingMode == ' + vip.renderingModes.edit + '">' +
						'<div vip-textbox class="inline-block" ng-model="_address.Street" ng-model-options="{debounce: 300}" placeholder="Street" wrap-with="span" edit-mode-class="span"></div>' +
						'<div vip-textbox class="inline-block" ng-model="_address.City" ng-model-options="{debounce: 300}" placeholder="City" wrap-with="span" edit-mode-class="span"></div>' +
						'<div vip-textbox class="inline-block" ng-model="_address.State" ng-model-options="{debounce: 300}" placeholder="State" wrap-with="span" edit-mode-class="span"></div>' +
						'<div vip-textbox class="inline-block" ng-model="_address.ZipCode" ng-model-options="{debounce: 300}" placeholder="ZipCode" wrap-with="span" edit-mode-class="span"></div>' +
					'</div>' +
					'<div class="inline-block vip-address__formatted-address">{{' + attrs.ngModel + '.FormattedAddress}}</div>' +
					'<div class="vip-map" style="display:none"></div>'
				);

				element.append(editElement);
				$compile(editElement)(scope);

				listeners.push(scope.$watch('_address.Street', function () {
					processAddress();
				}));
				listeners.push(scope.$watch('_address.City', function () {
					processAddress();
				}));
				listeners.push(scope.$watch('_address.State', function () {
					processAddress();
				}));
				listeners.push(scope.$watch('_address.ZipCode', function () {
					processAddress();
				}));

				listeners.push(scope.$watch('renderingMode', function (value) {
				}));


				listeners.push(scope.$on('$destroy', function () {
					mapTimeout = undefined;
					// unregister listeners
					for (var i = 0; i < listeners.length; i++)
						listeners[i]();
				}));
			}
		};
	}]);
});

