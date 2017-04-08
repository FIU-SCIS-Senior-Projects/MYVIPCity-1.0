define(['vip/js/vip', 'angular', 'jquery', 'lodash', 'select2'], function (vip, angular, jQuery, _) {
	'use strict';

	vip.directive('vipPromoterDropdown', ['$http', '$compile', function ($http, $compile) {
		return {
			restrict: 'ACE',

			require: 'ngModel',

			// new scope
			scope: {
				businessId: '='
			},

			link: function (scope, element, attrs, ngModelCtrl) {
				var listeners = [];
				var businessId, innerScope;
				var selectElement;

				var destroyInnerScope = function () {
					if (innerScope) {
						innerScope.$destroy();
						innerScope = null;
						element.empty();
						selectElement = undefined;
					}
				};

				var renderDropdown = function () {
					destroyInnerScope();
					var data;
					// make request to retrieve promoters
					$http.get('api/Business/' + businessId + '/Promoters').then(function (response) {
						data = response.data;
						// add id and text properties to each data item
						angular.forEach(data, function (item) {
							// Dont care about the business in this context
							item.Business = null;
							item.id = item.Id;
							item.text = item.FirstName + (item.NickName ? ' "' + item.NickName + '"' : '') + (item.LastName ? ' ' + item.LastName : '');
						});
						// create select element
						selectElement = angular.element('<select style="width: 100%"></select>');
						// append it to the parent
						element.append(selectElement);
						// create select2 widget
						jQuery(selectElement).select2({
							placeholder: attrs.placeholder || 'Select',
							allowClear: true,
							data: data,
							templateResult: function (item) {
								var newScope = scope.$new();
								newScope.promoter = item;
								var itemElement = angular.element(
									'<table class="vip-promoter-dropdown__dropdown-item">' +
										'<tr>' +
											'<td><div vip-promoter-picture ng-model="promoter" width="40" height="40" class="vip-existing-promoters__img-container" img-class="list-group__img img-circle vip-existing-promoters__img"></div></td>' +
											'<td><div vip-promoter-name ng-model="promoter" wrap-with="strong" vip-read-only></div></td>' +
										'<tr>' +
									'</table>'
								);
								$compile(itemElement)(newScope);
								return itemElement;
							}
						});

						jQuery(selectElement).val(null).trigger("change");


						jQuery(selectElement).on('change', function (e) {
							var selection = jQuery(this).select2('data');
							var value;
							if (!selection || !selection.length)
								value = null;
							else {
								value = _.find(data, { Id: parseInt(selection[0].id) });
							}
							ngModelCtrl.$setViewValue(value);
						});
					});
				};

				ngModelCtrl.$render = function() {
					if (!ngModelCtrl.$modelValue && selectElement) {
						jQuery(selectElement).val(null).trigger("change");
					}
				};

				// watches for business id
				listeners.push(scope.$watch('businessId', function (id) {
					if (id && id !== businessId) {
						businessId = id;
						renderDropdown();
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