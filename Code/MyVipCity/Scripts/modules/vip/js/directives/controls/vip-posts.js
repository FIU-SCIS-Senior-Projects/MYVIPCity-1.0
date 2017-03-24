define(['vip/js/vip', 'angular', 'lodash', 'sweet-alert'], function (vip, angular, _, swal) {
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
					'<button class="btn vip-posts__post-picture-btn" ng-click="clickPostPicture($event)"><i class="zmdi zmdi-image"></i>Post Picture</button>' +
					'<button class="btn vip-posts__post-video-btn"><i class="zmdi zmdi-play"></i>Post Video</button>' +
					'<button class="btn vip-posts__post-comment-btn" ng-click="clickPostComment($event)"><i class="zmdi zmdi-comment"></i>Post Comment</button>' +
				'</div>' +
				'<div class="card">' +
					'<div class="card__body">' +

						'<form name="postCommentForm" ng-show="_showPost == \'C\'">' +
							'<div required vip-textarea ng-model="_commentPost.Comment" edit-mode-class="form-control textarea-autoheight" vip-edit-only maxlength="1000" placeholder="Write your comment here..."></div>' +
							'<button class="btn vip-posts__save-post-btn" ng-disabled="postCommentForm.$invalid" ng-click="addPost(_commentPost)">Post Comment</button>' +
						'</form>' +

						'<form name="postCommentForm" ng-show="_showPost == \'P\'">' +

							'<button class="btn vip-posts__save-post-btn" ng-disabled="postCommentForm.$invalid" ng-click="addPost(_picturePost)">Post Picture</button>' +
						'</form>' +

					'</div>' +
				'</div>' +
				// '<div infinite-scroll="loadMorePosts()" infinite-scroll-distance="0" infinite-scroll-immediate-check="false">' +
				'<div class="vip-posts__posts">' +
					'<div class="card {{::post.PostType}}" ng-repeat="post in posts track by post.Id">' +
						'<div class="card__body">' +
							'<div vip-post ng-model="post"></div>' +
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
					// clear new posts
					scope._commentPost = { Comment: null, PostType: 'CommentPost' };
					scope._picturePost = { Comment: null, Pictures: [], PostType: 'PicturePost' };
					scope._showPost = null;

					// TODO: Refresh posts
				};

				scope.clickPostComment = function () {
					scope._commentPost = { Comment: null, PostType: 'CommentPost' };
					scope._showPost = vip.postsTypes.comment;
				};

				scope.clickPostPicture = function () {
					scope._picturePost = { Comment: null, Pictures: [], PostType: 'PicturePost' };
					scope._showPost = vip.postsTypes.picture;
				};

				// adds a new post
				scope.addPost = function (post) {
					postsManager.savePost(entityId, post).then(function () {
						afterPostAdded();
					});
				};

				// updates an existing post
				scope.updatePost = function (post) {
					return postsManager.savePost(entityId, post);
				}

				// deletes an exising post
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

			require: 'ngModel',

			priority: 10,

			link: function (scope, element, attrs, ngModelCtrl) {
				var listeners = [];
				var innerScope;

				var initialize = function () {
					// make a copy of the original post (so that it can be restored if cancel edit) TODO: Do this only if editing is allowed
					var originalPost = angular.copy(ngModelCtrl.$modelValue);
					// get the factory for the type of post (post.PostType)
					var postFactory = vipFactoryService(ngModelCtrl.$modelValue.PostType);
					// build the actual post element
					var postElement = postFactory.buildElement(attrs.ngModel);
					// set rendering mode to read
					scope.renderingMode = vip.renderingModes.read;

					// TODO: Do not compile buttons when editing is not allowed
					var content = angular.element(
						'<div>' +
							'<span class="vip-post__posted-on" ng-cloak>{{::' + attrs.ngModel + '.PostedOn | date: \'short\'}}</span>' +
							'<div class="actions pull-right">' +
								'<a href="" title="Edit" ng-click="edit()"><i class="zmdi zmdi-edit" ng-show="renderingMode == ' + vip.renderingModes.read + '"></i></a>' +
								'<a href="" title="Delete" ng-click="delete()"><i class="zmdi zmdi-delete"></i></a>' +
							'</div>' +
							'<form name="formPost">' +
							'</form>' +
							'<div class="vip-post__footer">' +
								'<button class="btn btn-primary vip-post__save-btn" ng-click="update()" ng-disabled="formPost.$invalid" ng-hide="renderingMode == ' + vip.renderingModes.read + '">Save</button>' +
								'<button class="btn btn-secondary vip-post__cancel-btn" ng-click="cancel()" ng-hide="renderingMode == ' + vip.renderingModes.read + '">Cancel</button>' +
							'</div>' +
						'</div>'
					);
					// append the post element to the content element under the form element
					content.find('form').append(postElement);
					// append the post content
					element.append(content);
					// get a new scope
					innerScope = scope.$new();
					// compile it
					$compile(content)(scope);

					// handles edit
					scope.edit = function () {
						// set edit rendering mode
						scope.renderingMode = vip.renderingModes.edit;
					};

					// deletes the post
					scope.delete = function () {
						// call delete post on parent scope
						scope.deletePost(ngModelCtrl.$viewValue);
					};

					// updates the post
					scope.update = function () {
						// save the post by calling savePost() in parent scope
						scope.updatePost(ngModelCtrl.$viewValue).then(function () {
							// set rendering mode back to read
							scope.renderingMode = vip.renderingModes.read;
							// make a copy of the new post
							originalPost = angular.copy(ngModelCtrl.$modelValue);
						}, function () {
							swal('Oops', 'An error has occurred updating the post.', 'error');
						});
					};

					// handles cancel
					scope.cancel = function () {
						// set read rendering mode
						scope.renderingMode = vip.renderingModes.read;
						// restore the post
						scope.post = angular.extend(scope.post, originalPost);
					};
				};

				var destroyInnerScope = function() {
					if (innerScope) {
						innerScope.$destroy();
						innerScope = null;
						element.empty();
					}
				};

				// define $render function
				ngModelCtrl.$render = function () {
					// destroy the inner scope in case it exists
					destroyInnerScope();
					// initialize the directive
					initialize();
				};

				listeners.push(scope.$on('$destroy', function () {
					destroyInnerScope();
					// unregister listeners
					for (var i = 0; i < listeners.length; i++)
						listeners[i]();
				}));
			}
		};
	}]);
});