define(['vip/js/vip'], function (vip) {
	'use strict';

	vip.postsTypes = {
		comment: 'C',
		video: 'V',
		picture: 'P'
	};

	vip.directive('vipPosts', ['$http', function ($http) {
		return {
			restrict: 'ACE',

			// new scope
			scope: true,

			template:
				'<div class="vip-posts__post-actions">' +
					'<button class="btn vip-posts__post-picture-btn"><i class="zmdi zmdi-image"></i>Post Picture</button>' +
					'<button class="btn vip-posts__post-video-btn"><i class="zmdi zmdi-play"></i>Post Video</button>' +
					'<button class="btn vip-posts__post-comment-btn" ng-click="clickPostComment($event)"><i class="zmdi zmdi-comment"></i>Post Comment</button>' +
				'</div>' +
				'<div class="card">' +
					'<div class="card__body">' +
						'<form name="postCommentForm" ng-show="_showPost == \'C\'">' +
							'<div required vip-textarea ng-model="_commentPost.Comment" edit-mode-class="form-control textarea-autoheight" vip-edit-only maxlength="1000" placeholder="Write your comment here..."></div>' +
							'<button class="btn vip-posts__save-post-btn" ng-disabled="postCommentForm.$invalid" ng-click="saveCommentPost($event)">Post</button>' +
						'</form>' +
					'</div>' +
				'</div>' +
				// '<div infinite-scroll="loadMorePosts()" infinite-scroll-distance="0" infinite-scroll-immediate-check="false">' +
				'<div class="vip-posts__posts">' +
					'<div class="card {{::post.PostType}}" ng-repeat="post in posts track by post.Id">' +
						'<div class="card__body">' +
							'<div vip-post></div>' +
						'</div>' +
					'</div>' +
					'<div class="load-more">' +
                        '<a href="" ng-click="loadMorePosts()"><i class="zmdi zmdi-refresh-alt"></i> Load more</a>' +
                    '</div>' +
				'</div>'
				,

			link: function (scope) {
				var listeners = [];

				var topLoad = 3;
				var businessId;
				scope.posts = [];
				scope.loadingPosts = false;

				// load more posts
				scope.loadMorePosts = function () {
					// check if there is not a loading operation currently in progress
					if (!scope.loadingPosts) {
						// indicate a new loading operation
						scope.loadingPosts = true;
						// get the url to retrieve the posts
						var url = ('api/Business/Posts/' + businessId + '/' + topLoad) + (scope.posts === null || !scope.posts.length ? '' : '/' + scope.posts[scope.posts.length - 1].Id);
						// make request to retrieve the posts
						$http.get(url).then(function (response) {
							// update posts array
							if (response.data) {
								scope.posts = scope.posts.concat(response.data);
								// TODO: Should we scroll to after the element before adding the new posts, i.e current 5, added 3, scroll to item 6
							}
						})['finally'](function () {
							// indicate that loading operation has finished
							scope.loadingPosts = false;
						});
					}
				};

				// refresh the posts
				var refreshPosts = function () {
					// set empty posts
					scope.posts = [];
					// load more posts
					scope.loadMorePosts();
				}

				var afterPostAdded = function () {
					scope._commentPost = { Comment: null };
					scope._showPost = null;

					// TODO: Refresh posts
				};

				scope.clickPostComment = function () {
					scope._commentPost = { Comment: null };
					scope._showPost = vip.postsTypes.comment;
				};

				scope.saveCommentPost = function () {
					$http.post('api/Business/' + businessId + '/PostComment', scope._commentPost).then(function () {
						afterPostAdded();
					});
				};

				var unregister = scope.$watch('model.Id', function (id) {
					if (id) {
						businessId = id;
						refreshPosts();
						unregister();
					}
				});

				listeners.push(scope.$on('$destroy', function () {
					// unregister listeners
					for (var i = 0; i < listeners.length; i++)
						listeners[i]();
				}));
			}
		};
	}]);

	vip.directive('vipPost', [function () {
		return {
			restrict: 'ACE',

			// new scope
			scope: true,

			template: '{{post.Comment}}',

			link: function(scope, element, attrs) {
				
			}
		};
	}]);
});