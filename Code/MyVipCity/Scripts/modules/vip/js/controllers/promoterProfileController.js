define(['vip/js/vip', 'sweet-alert', 'moment', 'swearjar'], function (vip, swal, moment, swearjar) {
	'use strict';

	vip.controller('vip.promoterProfileController', ['$scope', '$routeParams', '$http', '$location', 'vipConfig', '$route', 'vipUserService', function ($scope, $routeParams, $http, $location, vipConfig, $route, vipUserService) {
		// set rendering mode to read mode by default
		$scope.renderingMode = vip.renderingModes.read;
		// get the id of the profile from the route parameters
		var promoterProfileId = $routeParams.promoterProfileId;
		// set empty model
		$scope.model = {};
		// set reviews
		$scope.reviews = [];
		// set new review
		$scope.newReview = {};
		var top = 5;

		$scope.editClick = function () {
			$scope.renderingMode = vip.renderingModes.edit;
			$scope.showReadModeButton = true;
			$scope.showEditButton = false;
			$scope.showSaveButton = true;
		};

		$scope.readModeClick = function () {
			$scope.renderingMode = vip.renderingModes.read;
			$scope.showReadModeButton = false;
			$scope.showEditButton = true;
			$scope.showSaveButton = false;
		};

		var canEditPromoterProfile = function () {
			$scope.canEditPromoterProfile = true;
			$scope.showReadModeButton = false;
			$scope.showEditButton = true;
		};

		$scope.reviewsTabShow = function () {
		};

		// handles add review click
		$scope.addReview = function () {
			// make sure the user is authenticated
			if (!vipUserService.IsAuthenticated()) {
				swal('Log in first', 'You need to login to leave a review. You can register if you do not have an account yet.', 'info');
				return;
			}
			// flag to indicate a review is being added
			$scope.addingReview = true;
			// reset the review
			$scope.newReview = {};
		};

		// handles cancel review click
		$scope.cancelReview = function () {
			$scope.addingReview = false;
		};

		// loads more review
		$scope.loadMoreReviews = function () {
			var url = ('api/PromoterProfile/Reviews/' + promoterProfileId + '/' + top) + ($scope.reviews === null || !$scope.reviews.length ? '' : '/' + $scope.reviews[$scope.reviews.length - 1].Id);
			$http.get(url).then(function (response) {
				if (response.data) {
					$scope.reviews = $scope.reviews.concat(response.data);
				}
			});
		}

		// refresh the reviews
		var refreshReviews = function () {
			// set reviews
			$scope.reviews = [];
			$scope.loadMoreReviews();
		}

		refreshReviews();

		var showErrorPopup = function (title, errorMsg) {
			swal(title || 'Oops', errorMsg || 'An error has occurred', 'error');
		};

		// removes a review
		$scope.removeReview = function (e, review) {
			swal({
				type: 'warning',
				title: 'Delete review?',
				text: 'Are you sure you want to delete this review?',
				confirmButtonText: "Yes, delete it",
				showCancelButton: true
			}).then(function () {
				$http.delete('api/PromoterProfile/Review/' + review.Id).then(function (response) {
					if (response.data.Result) {
						// decrease number of reviews
						$scope.model.ReviewsCount--;
						// remove the review from the list
						_.remove($scope.reviews, function (r) {
							return r.Id === review.Id;
						});
					}
					else {
						var msg = (result.Messages || []).join("\n");
						showErrorPopup('Review was not deleted', msg);
					}
				}, function () {
					showErrorPopup();
				});
			}, angular.noop);
		};

		// handles submit review click
		$scope.submitReview = function () {
			// check if there is a review text
			if ($scope.newReview.Text) {
				// censor the text
				var cleanedText = swearjar.censor($scope.newReview.Text);
				// check if the text had indeed swear words
				if (cleanedText !== $scope.newReview.Text) {
					swal({
						type: 'error',
						title: 'Hey, watch your writing!',
						html: '<p>We have identified some inappropiate words in your review, please change the words masked with *.</p><i>' + cleanedText + '</i>'
					});
					return;
				}
			}

			// submit the review
			$http.post('api/PromoterProfile/Review/' + promoterProfileId, $scope.newReview).then(function (response) {
				// get the result of adding the review
				var result = response.data;
				// check if the review was not added
				if (!result.Result) {
					// show reason
					var msg = (result.Messages || []).join("\n");
					showErrorPopup('Review was not added', msg);
				}
				else {
					// review was added
					swal('Thank you!', "Your review has been added successfully.", 'success');
					$scope.addingReview = false;
					$scope.model.ReviewsCount++;
					refreshReviews();
				}
			}, function (error) {
				showErrorPopup('Oops!', 'Something went wrong adding the review!');
			});
		};

		$scope.save = function () {
			// make request to get the profile info
			$http.put('api/PromoterProfile', $scope.model).then(function (response) {
				// let the user know save was successful
				swal({
					type: 'success',
					title: 'Success!',
					text: 'Your profile has been updated successfully',
					confirmButtonText: "Ok"
				}).then(function () {
					$scope.$apply(function () {
						// refresh profile page
						$route.reload();
					});
				}, function () {
					$route.reload();
				});
			}, function (error) {
				showErrorPopup('Oops!', 'Something went wrong!');
			});
		};

		// assumes that this function is invoked only if the current user is the owner of this profile
		var showWelcomeToProfilePopup = function (promoter) {
			// get the date when the promoter profile was created
			var createdOn = promoter.CreatedOn;
			var then = moment(createdOn);
			var now = moment();
			var difference = now.diff(then);
			var duration = moment.duration(difference);
			var seconds = duration.asSeconds();
			// if the promoter profile was created no more than 5 seconds ago, then show a welcome to profile popup
			if (seconds < 8) {
				swal({
					type: 'success',
					title: 'Welcome to your new profile for ' + promoter.Business.Name + '!',
					html: '<br/><p>Please add an awesome profile picture and some information about yourself. <i>That is what user will see</i>.</p>' +
						'<p>You can also start posting your activity under the <strong>POST</strong> section.</p>',
				});
			}
		};

		// make request to get the profile info
		$http.get('api/PromoterProfile/' + promoterProfileId).then(function (response) {
			// if no response data then redirect to home
			if (!response.data) {
				$location.path('/');
			}
			else {
				// update the model
				$scope.model = response.data;
				// check if there is a list of the ids of all the profiles owned by the current user
				if (vipConfig.PromoterProfileIds && vipConfig.PromoterProfileIds.indexOf($scope.model.Id) > -1) {
					canEditPromoterProfile();
					showWelcomeToProfilePopup($scope.model);
				}
				// check if the user is an admin
				if (vipUserService.isAdmin()) {
					$scope.canRemoveReviews = true;
				}
			}
		});




		$scope.toggleRenderingMode = function () {
			if ($scope.renderingMode === vip.renderingModes.read)
				$scope.renderingMode = vip.renderingModes.edit;
			else
				$scope.renderingMode = vip.renderingModes.read;
		};
	}]);
});