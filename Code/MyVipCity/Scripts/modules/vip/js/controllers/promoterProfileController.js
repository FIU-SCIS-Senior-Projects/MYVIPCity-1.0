define(['vip/js/vip', 'sweet-alert'], function (vip, swal) {
	'use strict';

	vip.controller('vip.promoterProfileController', ['$scope', '$routeParams', '$http', '$location', 'vipConfig', '$route', function ($scope, $routeParams, $http, $location, vipConfig, $route) {
		// set rendering mode to read mode by default
		$scope.renderingMode = vip.renderingModes.read;
		// get the id of the profile from the route parameters
		var promoterProfileId = $routeParams.promoterProfileId;
		// set empty model
		$scope.model = {};

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
				swal('Oops!', 'Something went wrong!', 'error');
			});
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
				}
				// TODO: fix retrieval of average rating
				$scope.model.AverageRating = 5;

				//$scope.model.ProfilePicture = {
				//	Picture: {
				//		BinaryDataId: 82
				//	},
				//	CropData: '{"x":408.2655655175196,"y":0,"width":320.2106657759608,"height":320.2106657759608,"rotate":0,"scaleX":1,"scaleY":1}'
				//};
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