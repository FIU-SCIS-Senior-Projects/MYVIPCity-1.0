define(['vip/js/vip', 'angular'], function (vip, angular) {
	'use strict';

	vip.config(['vipFactoryServiceProvider', function (vipFactoryServiceProvider) {
		vipFactoryServiceProvider.registerFactory(['video-post', 'VideoPost', 'VideoPostDto'], [function () {
			return {
				buildElement: function (ngModelBind) {
					var element = angular.element(
						'<div required vip-video-control ng-model="' + ngModelBind + '.VideoUrl" edit-mode-class="form-control"></div>' +
						'<div vip-textarea ng-model="' + ngModelBind + '.Comment" edit-mode-class="form-control textarea-autoheight" maxlength="1000" placeholder="Enter caption here..."></div>'
					);
					return element;
				}
			};
		}]);
	}]);
});