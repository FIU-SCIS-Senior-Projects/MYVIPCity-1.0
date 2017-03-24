define(['vip/js/vip', 'angular'], function (vip, angular) {
	'use strict';

	vip.config(['vipFactoryServiceProvider', function (vipFactoryServiceProvider) {
		vipFactoryServiceProvider.registerFactory('business-posts-manager', ['$http', '$log', '$q', function ($http, $log, $q) {
			return {
				getLoadPostsUrl: function (businessId, top, lastPostId) {
					var url = ('api/Business/Posts/' + businessId + '/' + top) + (angular.isUndefined(lastPostId) ? '' : '/' + lastPostId);
					return url;
				},

				savePost: function (businessId, post) {
					var savePromise;

					switch (post.PostType) {
						case 'CommentPost':
							savePromise = $http.post('api/Business/' + businessId + '/PostComment', post);
							break;
						case 'PicturePost':
							savePromise = $http.post('api/Business/' + businessId + '/PostPicture', post);
							break;
						case 'VideoPost':
							savePromise = $http.post('api/Business/' + businessId + '/PostVideo', post);
							break;
						default:
							$log.error('Unknown PostType = "' + post.PostType + '"');
							return $q.reject();
					}

					return savePromise;
				},

				deletePost: function (businessId, postId) {
					return $http.delete('api/Business/' + businessId + '/DeletePost/' + postId);
				}
			};
		}]);
	}]);
});