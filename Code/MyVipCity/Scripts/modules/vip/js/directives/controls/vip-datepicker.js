define(['vip/js/vip', 'moment', 'angular'], function(vip, moment) {
	'use strict';

	vip.directive('vipDatepicker', function() {
		return {
			// use as class or element
			restrict: 'ACE',

			// require ngModel
			require: 'ngModel',

			// isolated scope
			scope: {
				vipMinDate: '=',
				vipMaxDate: '='
			},

			// template of directive, delegate on angular material date-picker
			template: '<md-datepicker ng-model="_model" ng-change="dateChanged(_model)" md-min-date="vipMinDate" md-max-date="vipMaxDate"></md-datepicker>',

			compile: function(tElement, tAttrs) {
				// add placeholder attribute if necessary
				if (tAttrs.vipPlaceholder) {
					tElement.find('md-datepicker').attr('md-placeholder', tAttrs.vipPlaceholder);
				}
				if (tAttrs.Name) {
					tElement.find('md-datepicker').attr('name', tAttrs.name);
					tElement.removeAttr('name');
				}
				if (angular.isDefined(tAttrs.required)) {
					tElement.find('md-datepicker').attr('required', true);
				}
				// return link function
				return function linkFunction(scope, element, attrs, ngModelCtrl) {
					// define a formatter to receive a date in ISO 8601 date string and return a Javascript Date
					ngModelCtrl.$formatters.unshift(function(isoDate) {
						var momentDate = moment(isoDate);
						return isoDate && momentDate.isValid() ? momentDate.toDate() : undefined;
					});
					// define a parser to receive a JS Date and return an ISO 8601 date string
					ngModelCtrl.$parsers.push(function(date) {
						var isoDate = moment(date).toISOString();
						return isoDate;
					});
					// set _model value
					ngModelCtrl.$render = function() {
						scope._model = ngModelCtrl.$viewValue;
					};
					// handler when _model changed (by user interaction)
					scope.dateChanged = function(date) {
						ngModelCtrl.$setViewValue(date);
					};
				};
			}
		};
	});
});
