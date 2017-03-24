define(['vip/js/vip', 'angular'], function (vip, angular) {
	'use strict';

	vip.config(['vipFactoryServiceProvider', function (vipFactoryServiceProvider) {
		vipFactoryServiceProvider.registerFactory(['picture-post', 'PicturePost', 'PicturePostDto'], [function () {
			return {
				buildElement: function (ngModelBind) {
					var element = angular.element(
						'<div required vip-images-control ng-model="' + ngModelBind + '.Pictures"></div>' +
						'<div vip-textarea ng-model="' + ngModelBind + '.Comment" edit-mode-class="form-control textarea-autoheight" maxlength="1000" placeholder="Enter a caption here..."></div>'
					);
					return element;
				}
			};
		}]);
	}]);
});