define(['vip/js/vip', 'angular'], function (vip, angular) {
	'use strict';

	vip.config(['vipFactoryServiceProvider', function (vipFactoryServiceProvider) {
		vipFactoryServiceProvider.registerFactory('promoter-posts-manager', ['$http', '$log', '$q', function ($http, $log, $q) {
			var wrapHttpPromise = function (httpPromise) {
				return httpPromise.then(function (response) {
					return response.data;
				}, function (error) {
					return $q.reject(error);
				});
			};

			return {
				getLoadPostsUrl: function (promoterProfileId, top, lastPostId) {
					var url = ('api/PromoterProfile/Posts/' + promoterProfileId + '/' + top) + (angular.isUndefined(lastPostId) ? '' : '/' + lastPostId);
					return url;
				},

				savePost: function (promoterProfileId, post) {
					var savePromise;

					switch (post.PostType) {
						case 'CommentPostDto':
							savePromise = wrapHttpPromise($http.post('api/PromoterProfile/' + promoterProfileId + '/PostComment', post));
							break;
						case 'PicturePostDto':
							savePromise = wrapHttpPromise($http.post('api/PromoterProfile/' + promoterProfileId + '/PostPicture', post));
							break;
						case 'VideoPostDto':
							savePromise = wrapHttpPromise($http.post('api/PromoterProfile/' + promoterProfileId + '/PostVideo', post));
							break;
						default:
							$log.error('Unknown PostType = "' + post.PostType + '"');
							return $q.reject();
					}

					return savePromise;
				},

				deletePost: function (promoterProfileId, postId) {
					return wrapHttpPromise($http.delete('api/PromoterProfile/' + promoterProfileId + '/DeletePost/' + postId));
				}
			};
		}]);
	}]);
});