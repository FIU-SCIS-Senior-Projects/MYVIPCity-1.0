define(['vip/js/vip', 'angular'], function (vip, angular) {
	'use strict';

	vip.config(['vipFactoryServiceProvider', function (vipFactoryServiceProvider) {
		vipFactoryServiceProvider.registerFactory(['comment-post', 'CommentPost', 'CommentPostDto'], [function () {
			return {
				buildElement: function (ngModelBind) {
					var element = angular.element(
						'<div required vip-textarea ng-model="' + ngModelBind + '.Comment" edit-mode-class="form-control textarea-autoheight" maxlength="1000" placeholder="Write your comment here..."></div>'
					);
					return element;
				}
			};
		}]);
	}]);
});