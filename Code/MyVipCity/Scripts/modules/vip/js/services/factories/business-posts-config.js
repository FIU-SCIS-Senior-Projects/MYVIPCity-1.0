define(['vip/js/vip', 'angular'], function (vip, angular) {
	'use strict';

	vip.config(['vipFactoryServiceProvider', function (vipFactoryServiceProvider) {
		vipFactoryServiceProvider.registerFactory('business-posts-config', [function () {
			return {
				getLoadPostsUrl : function(entityId, top, lastPostId) {
					var url = ('api/Business/Posts/' + entityId + '/' + top) + (angular.isUndefined(lastPostId) ? '' : '/' + lastPostId);
					return url;
				}
			};
		}]);
	}]);
});