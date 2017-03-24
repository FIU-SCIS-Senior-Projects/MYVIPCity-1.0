define(['vip/js/vip', 'angular'], function (vip, angular) {
	'use strict';

	vip.config(['vipFactoryServiceProvider', function (vipFactoryServiceProvider) {
		vipFactoryServiceProvider.registerFactory('business-posts-manager', ['$http', function ($http) {
			return {
				getLoadPostsUrl : function(businessId, top, lastPostId) {
					var url = ('api/Business/Posts/' + businessId + '/' + top) + (angular.isUndefined(lastPostId) ? '' : '/' + lastPostId);
					return url;
				},

				savePost: function (businessId, post) {
					var savePromise;

					if (post.PostType === 'CommentPost') {
						// if the post
						if (!post.Id) {
							$http.post('api/Business/' + businessId + '/PostComment', post);
						}
					}
				}
			};
		}]);
	}]);
});