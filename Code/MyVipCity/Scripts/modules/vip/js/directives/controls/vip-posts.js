define(['vip/js/vip', 'angular', 'lodash', 'sweet-alert'], function (vip, angular, _, swal) {
	'use strict';

	vip.postsTypes = {
		comment: 'C',
		video: 'V',
		picture: 'P'
	};

	/*
		Directive to maintain a collection of posts 
	*/
	vip.directive('vipPosts', ['$http', '$log', 'vipFactoryService', function ($http, $log, vipFactoryService) {
		return {
			restrict: 'ACE',

			// isolated scope
			scope: {
				entityId: '=',
				readModeOnly: '=vipReadOnly'
			},

			template:
				// add post buttons
				'<div class="vip-posts__post-actions" ng-if="!readModeOnly">' +
					'<button class="btn vip-posts__post-picture-btn" ng-click="clickPostPicture($event)"><i class="zmdi zmdi-image"></i>Post Picture</button>' +
					'<button class="btn vip-posts__post-video-btn" ng-click="clickPostVideo($event)"><i class="zmdi zmdi-play"></i>Post Video</button>' +
					'<button class="btn vip-posts__post-comment-btn" ng-click="clickPostComment($event)"><i class="zmdi zmdi-comment"></i>Post Comment</button>' +
				'</div>' +
				// add post container
				'<div class="card" ng-show="_showPost">' +
					'<div class="card__body">' +
						'<div vip-post ng-model="_newPost" show-edit-button="false" show-delete-button="false" save-caption="Post" on-save-event="vipPostSave" on-cancel-event="vipPostCancel" vip-rendering-mode="' + vip.renderingModes.edit + '"></div>' +
					'</div>' +
				'</div>' +
				// existing posts
				'<div class="vip-posts__posts">' +
					'<div class="card {{::post.PostType}}" ng-repeat="post in posts track by post.Id" vip-random-bg-color>' +
						'<div class="card__body">' +
							'<div vip-post ng-model="post"></div>' +
						'</div>' +
					'</div>' +
					'<div class="load-more">' +
                        '<a href="" ng-click="loadMorePosts()" ng-disabled="loadingPosts"><i class="zmdi zmdi-refresh-alt"></i> Load more</a>' +
                    '</div>' +
				'</div>'
				,

			controller: ['$scope', '$attrs', function($scope, $attrs) {
				// store the list of posts
				$scope.posts = [];
				// indicate if posts are being loaded
				$scope.loadingPosts = false;
			}],

			link: function (scope, element, attrs) {
				if (!attrs.postsManagerId)
					$log.error('vip-posts directive, attribute "posts-manager-id" not specified');
				// set listeners array, see $destroy
				var listeners = [];
				// get the post manager
				var postsManager = vipFactoryService(attrs.postsManagerId);
				// number of post to load when load more is clicked
				var topLoad = 6;
				// id of the entity owner of the posts
				var entityId;

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

				var clearNewPosts = function () {
					scope._showPost = null;
				};

				var afterPostAdded = function (addedPost) {
					// insert added post at the beginning of the array
					scope.posts.splice(0, 0, addedPost);
					// clear new posts
					clearNewPosts();
				};

				scope.clickPostComment = function () {
					scope._newPost = { Comment: null, PostType: 'CommentPostDto' };
					scope._showPost = vip.postsTypes.comment;
				};

				scope.clickPostPicture = function () {
					scope._newPost = { Comment: null, Pictures: [], PostType: 'PicturePostDto' };
					scope._showPost = vip.postsTypes.picture;
				};

				scope.clickPostVideo = function () {
					scope._newPost = { Comment: null, VideoUrl: null, PostType: 'VideoPostDto' };
					scope._showPost = vip.postsTypes.video;
				};

				// listen for event to save new post
				listeners.push(scope.$on('vipPostSave', function (event, post) {
					// prevent default behavior of saving operation
					event.preventDefault();
					// stop event propagation
					if (event.stopPropagation)
						event.stopPropagation();
					// save the post
					postsManager.savePost(entityId, post).then(afterPostAdded, function () {
						swal('Oops', 'An error has occurred saving the post.', 'error');
					});
				}));

				// listen for event to cancel adding new post
				listeners.push(scope.$on('vipPostCancel', function (event) {
					// prevent default behavior of saving operation
					event.preventDefault();
					// stop event propagation
					if (event.stopPropagation)
						event.stopPropagation();
					clearNewPosts();
				}));

				// updates an existing post
				scope.updatePost = function (post) {
					return postsManager.savePost(entityId, post);
				}

				// deletes an exising post
				scope.deletePost = function (post) {
					postsManager.deletePost(entityId, post.Id).then(function () {
						// remove the post from the array of posts
						_.remove(scope.posts, function (p) {
							return p.Id === post.Id;
						});
					});
				};

				// watches for an entity id (this is the id of the entity owner of the posts)
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

	/*
		Directive to display a specific post
	*/
	vip.directive('vipPost', ['$compile', 'vipFactoryService', function ($compile, vipFactoryService) {
		return {
			restrict: 'ACE',

			// new scope
			scope: true,

			require: 'ngModel',

			link: function (scope, element, attrs, ngModelCtrl) {
				var listeners = [];
				var innerScope;

				scope.showEditButton = attrs.showEditButton !== 'false';
				scope.showDeleteButton = attrs.showDeleteButton !== 'false';
				scope.showSaveButton = attrs.showSaveButton !== 'false';
				scope.showCancelButton = attrs.showCancelButton !== 'false';
				scope.saveButtonCaption = attrs.saveCaption || 'Save';

				var initialize = function () {
					// make a copy of the original post (so that it can be restored if cancel edit) TODO: Do this only if editing is allowed
					var originalPost = angular.copy(scope.post);
					// get the factory for the type of post (post.PostType)
					var postFactory = vipFactoryService(scope.post.PostType);
					// build the actual post element
					var postElement = postFactory.buildElement(attrs.ngModel);
					// set initial rendering mode (read by default)
					scope.renderingMode = (vip.renderingModes.edit + '') === attrs.vipRenderingMode ? vip.renderingModes.edit : vip.renderingModes.read;

					// TODO: Do not compile buttons when editing is not allowed
					var content = angular.element(
						'<div>' +
							'<div class="vip-post__header">' +
								'<span class="vip-post__posted-on" ng-cloak>{{::post.PostedOn | date: \'short\'}}</span>' +
								'<div class="actions pull-right" ng-if="!readModeOnly">' +
									'<a href="" title="Edit" ng-click="edit()" ng-if="showEditButton"><i class="zmdi zmdi-edit" ng-show="renderingMode == ' + vip.renderingModes.read + '"></i></a>' +
									'<a href="" title="Delete" ng-click="delete()" ng-if="showDeleteButton"><i class="zmdi zmdi-delete"></i></a>' +
								'</div>' +
							'</div>' +
							'<form name="formPost">' +
							'</form>' +
							'<div class="vip-post__footer" ng-if="!readModeOnly">' +
								'<button class="btn btn-primary vip-post__save-btn" ng-click="_save(post)" ng-if="showSaveButton" ng-disabled="formPost.$invalid" ng-hide="renderingMode == ' + vip.renderingModes.read + '">{{::saveButtonCaption}}</button>' +
								'<button class="btn btn-secondary vip-post__cancel-btn" ng-click="cancel(post)" ng-if="showCancelButton" ng-hide="renderingMode == ' + vip.renderingModes.read + '">Cancel</button>' +
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
						scope.deletePost(scope.post);
					};

					// saves the post
					scope._save = function (post) {
						// check if we need to emit an event
						if (attrs.onSaveEvent) {
							var event = scope.$emit(attrs.onSaveEvent, post);
							if (event.defaultPrevented) {
								return;
							}
						}
						// save the post by calling updatePost() in parent scope
						scope.updatePost(post).then(function (savedPost) {
							angular.extend(post, savedPost);
							// set rendering mode back to read
							scope.renderingMode = vip.renderingModes.read;
							// make a copy of the new post
							originalPost = angular.copy(savedPost);
						}, function () {
							swal('Oops', 'An error has occurred saving the post.', 'error');
						});
					};

					// handles cancel
					scope.cancel = function (post) {
						// check if we need to emit an event
						if (attrs.onCancelEvent) {
							var event = scope.$emit(attrs.onCancelEvent, post);
							if (event.defaultPrevented) {
								return;
							}
						}
						// set read rendering mode
						scope.renderingMode = vip.renderingModes.read;
						// restore the post
						scope.post = angular.extend(scope.post, originalPost);
					};
				};

				// destroys the scope used to compile the specific post element
				var destroyInnerScope = function () {
					if (innerScope) {
						innerScope.$destroy();
						innerScope = null;
						element.empty();
					}
				};

				// define $render function
				ngModelCtrl.$render = function () {
					// save post in scope
					scope.post = ngModelCtrl.$modelValue;
					// destroy the inner scope in case it exists
					destroyInnerScope();
					// make sure there is a value
					if (scope.post) {
						// initialize the directive
						initialize();
					}
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