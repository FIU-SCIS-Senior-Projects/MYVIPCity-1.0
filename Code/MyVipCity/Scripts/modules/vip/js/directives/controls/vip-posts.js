define(['vip/js/vip', 'jquery', 'angular'], function (vip, jQuery, angular) {
	'use strict';

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
						'<form name="postCommentForm" ng-show="_showPostComment">' +
							'<div required vip-textarea ng-model="_commentPost.Comment" edit-mode-class="form-control textarea-autoheight" vip-edit-only maxlength="1000" placeholder="Write your comment here..."></div>' +
							'<button class="btn vip-posts__save-post-btn" ng-disabled="postCommentForm.$invalid" ng-click="saveCommentPost($event)">Post</button>' +
						'</form>' +
					'</div>' +
				'</div>'
				,

			link: function (scope) {
				var listeners = [];

				var afterPostAdded = function() {
					scope._commentPost = { Comment: null };
					scope._showPostComment = false;

					// TODO: Refresh posts
				};

				scope.clickPostComment = function () {
					scope._commentPost = { Comment: null };
					scope._showPostComment = true;
				};

				scope.saveCommentPost = function() {
					$http.post('api/Business/' + scope.model.Id + '/PostComment', scope._commentPost).then(function() {
						afterPostAdded();
					});
				};

				listeners.push(scope.$on('$destroy', function () {
					// unregister listeners
					for (var i = 0; i < listeners.length; i++)
						listeners[i]();
				}));
			}
		};
	}]);
});