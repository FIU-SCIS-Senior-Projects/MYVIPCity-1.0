define(['vip/js/vip', 'angular'], function (vip, angular) {
	'use strict';

	vip.config(['vipFactoryServiceProvider', function (vipFactoryServiceProvider) {
		vipFactoryServiceProvider.registerFactory('business-posts-manager', ['$http', '$log', '$q', function ($http, $log, $q) {
			var wrapHttpPromise = function (httpPromise) {
				return httpPromise.then(function (response) {
					return response.data;
				}, function (error) {
					return $q.reject(error);
				});
			};

			return {
				getLoadPostsUrl: function (businessId, top, lastPostId) {
					var url = ('api/Business/Posts/' + businessId + '/' + top) + (angular.isUndefined(lastPostId) ? '' : '/' + lastPostId);
					return url;
				},

				savePost: function (businessId, post) {
					var savePromise;

					switch (post.PostType) {
						case 'CommentPost':
							savePromise = wrapHttpPromise($http.post('api/Business/' + businessId + '/PostComment', post));
							break;
						case 'PicturePost':
							savePromise = wrapHttpPromise($http.post('api/Business/' + businessId + '/PostPicture', post));
							break;
						case 'VideoPost':
							savePromise = wrapHttpPromise($http.post('api/Business/' + businessId + '/PostVideo', post));
							break;
						default:
							$log.error('Unknown PostType = "' + post.PostType + '"');
							return $q.reject();
					}

					return savePromise;
				},

				deletePost: function (businessId, postId) {
					return wrapHttpPromise($http.delete('api/Business/' + businessId + '/DeletePost/' + postId));
				}
			};
		}]);
	}]);
});