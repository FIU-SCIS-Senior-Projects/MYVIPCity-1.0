define(['vip/js/vip', 'angular', 'lodash'], function (vip, angular, _) {
	'use strict';

	vip.postsTypes = {
		comment: 'C',
		video: 'V',
		picture: 'P'
	};

	vip.directive('vipPosts', ['$http', '$log', 'vipFactoryService', function ($http, $log, vipFactoryService) {
		return {
			restrict: 'ACE',

			// isolated scope
			scope: {
				entityId: '='
			},

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
							'<button class="btn vip-posts__save-post-btn" ng-disabled="postCommentForm.$invalid" ng-click="saveCommentPost(_commentPost)">Post</button>' +
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

			link: function (scope, element, attrs) {
				if (!attrs.postsManagerId)
					$log.error('vip-posts directive, attribute "posts-manager-id" not specified');
				// get the post manager
				var postsManager = vipFactoryService(attrs.postsManagerId);

				var listeners = [];

				var topLoad = 3;
				var entityId;
				scope.posts = [];
				scope.loadingPosts = false;

				// load more posts
				scope.loadMorePosts = function () {
					// check if there is not a loading operation currently in progress
					if (!scope.loadingPosts) {
						// indicate a new loading operation is in progress (about to start)
						scope.loadingPosts = true;
						// get the url to retrieve the posts
						var url = postsManager.getLoadPostsUrl(entityId, topLoad, scope.posts && scope.posts.length ? scope.posts[scope.posts.length - 1].Id : undefined);
						// make request to retrieve the posts
						$http.get(url).then(function (response) {
							// update posts array
							if (response.data) {
								// add posts at the end of the array
								scope.posts = scope.posts.concat(response.data);
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
					scope._commentPost = { Comment: null, PostType: 'CommentPost' };
					scope._showPost = null;

					// TODO: Refresh posts
				};

				scope.clickPostComment = function () {
					scope._commentPost = { Comment: null, PostType: 'CommentPost' };
					scope._showPost = vip.postsTypes.comment;
				};

				scope.saveCommentPost = function (post) {
					postsManager.savePost(entityId, post).then(function () {
						afterPostAdded();
					});
				};

				scope.deletePost = function (post) {
					postsManager.deletePost(entityId, post.Id).then(function () {
						// remove the removed post from the array of posts
						_.remove(scope.posts, function (p) {
							return p.Id === post.Id;
						});
					});
				};

				var unregister = scope.$watch('entityId', function (id) {
					if (id) {
						entityId = id;
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

	vip.directive('vipPost', ['$compile', 'vipFactoryService', function ($compile, vipFactoryService) {
		return {
			restrict: 'ACE',

			// new scope
			scope: true,

			link: function (scope, element) {
				// make a copy of the original post (so that it can be restored if cancel edit) TODO: Do this only if editing is allowed
				var originalPost = angular.copy(scope.post);
				// get the factory for the type of post (post.PostType)
				var postFactory = vipFactoryService(scope.post.PostType);
				// build the actual post element
				var postElement = postFactory.buildElement('post');
				// set rendering mode to read
				scope.renderingMode = vip.renderingModes.read;

				// TODO: Do not compile buttons when editing is not allowed
				var content = angular.element(
					'<div>' +
						'<span class="vip-post__posted-on">{{::post.PostedOn | date: \'short\'}}</span>' +
						'<div class="actions pull-right">' +
							'<a href="" title="Edit" ng-click="edit()"><i class="zmdi zmdi-edit" ng-show="renderingMode == ' + vip.renderingModes.read + '"></i></a>' +
							'<a href="" title="Delete" ng-click="deletePost(post)"><i class="zmdi zmdi-delete"></i></a>' +
						'</div>' +
						'<form name="formPost">' +
						'</form>' +
						'<div class="vip-post__footer">' +
							'<button class="btn btn-primary vip-post__save-btn" ng-disabled="formPost.$invalid" ng-hide="renderingMode == ' + vip.renderingModes.read + '">Save</button>' +
							'<button class="btn btn-secondary vip-post__cancel-btn" ng-click="cancel()" ng-hide="renderingMode == ' + vip.renderingModes.read + '">Cancel</button>' +
						'</div>' +
					'</div>'
				);
				// append the post element to the content element under the form element
				content.find('form').append(postElement);
				// append the post content
				element.append(content);
				// compile it
				$compile(content)(scope);

				// handles edit
				scope.edit = function () {
					// set edit rendering mode
					scope.renderingMode = vip.renderingModes.edit;
				};


				// handles cancel
				scope.cancel = function () {
					// set read rendering mode
					scope.renderingMode = vip.renderingModes.read;
					// restore the post
					scope.post = angular.extend(scope.post, originalPost);
				};
			}
		};
	}]);
});