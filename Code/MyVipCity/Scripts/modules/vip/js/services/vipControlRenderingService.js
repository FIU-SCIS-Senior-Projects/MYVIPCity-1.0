define(['vip/js/vip', 'angular', 'jquery'], function (vip, angular, jQuery) {
	'use strict';

	// This is used to render read and edit mode versions of a control
	vip.factory('vipControlRenderingService', ['$compile', function ($compile) {
		return function (scope, element) {
			return {
				$$editModeElement: null,
				$$readModeElement: null,
				$$createReadModeElementFn: angular.noop,
				$$createEditModeElementFn: angular.noop,

				$$hideEditModeElement: function () {
					if (this.$$editModeElement)
						jQuery(this.$$editModeElement).hide();
				},

				$$hideReadModeElement: function () {
					if (this.$$readModeElement)
						jQuery(this.$$readModeElement).hide();
				},

				$$showEditModeElement: function () {
					if (this.$$editModeElement)
						jQuery(this.$$editModeElement).show();
				},

				$$showReadModeElement: function () {
					if (this.$$readModeElement)
						jQuery(this.$$readModeElement).show();
				},

				$$showReadMode: function () {
					this.$$hideEditModeElement();
					if (this.$$readModeElement) {
						this.$$showReadModeElement();
					}
					else {
						this.$$readModeElement = this.$$createReadModeElementFn();
						element.append(this.$$readModeElement);
						$compile(this.$$readModeElement)(scope);
					}
				},

				$$showEditMode: function () {
					this.$$hideReadModeElement();
					if (this.$$editModeElement) {
						this.$$showEditModeElement();
					}
					else {
						this.$$editModeElement = this.$$createEditModeElementFn();
						element.append(this.$$editModeElement);
						$compile(this.$$editModeElement)(scope);
					}
				},

				setRenderingMode: function (renderingMode) {
					if (renderingMode === vip.renderingModes.read)
						this.$$showReadMode();
					else if (renderingMode === vip.renderingModes.edit)
						this.$$showEditMode();
				},

				setCreateReadModeElementFunction: function (fn) {
					this.$$createReadModeElementFn = fn;
				},

				setCreateEditModeElementFunction: function (fn) {
					this.$$createEditModeElementFn = fn;
				}
			};
		};
	}]);
});