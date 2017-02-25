define(['vip/js/vip', 'sweet-alert'], function (vip, swal) {
	'use strict';

	vip.controller('vip.addBusinessController', ['$scope', 'vipFactoryService', '$q', '$http', 'vipLocationService', function ($scope, vipFactoryService, $q, $http, vipLocationService) {
		// TODO Remove this button
		$scope.showToggleButton = true;
		$scope.showSaveButton = true;
		$scope.renderingMode = vip.renderingModes.edit;
		$scope.model = {};

		var setNewBusinessModel = function () {
			$q.when(vipFactoryService('business')).then(function (newBusiness) {
				$scope.model = newBusiness;
			});
		};

		setNewBusinessModel();

		$scope.save = function () {
			$http.post('api/Business', $scope.model)
				.then(function (response) {
					var business = response.data;
					// let the user know the registration was successful
					swal({
						type: 'success',
						title: 'Club created successfully!',
						text: 'Do you want to view the club?',
						showCancelButton: true,
						confirmButtonText: "Yes",
						cancelButtonText: "No"
					}).then(function () {
						vipLocationService.goToClub(business.FriendlyId);
					}, setNewBusinessModel);
				}, function (error) {

				});
		};

		//var newBusiness = function () {
		//	return {
		//		Name: 'Dance by the Ocean Nightclub',
		//		Phrase: 'People, food, music and much more...',
		//		Parking: 'Street/Valet',
		//		Ambiance: 'Upscale Trendy',
		//		Alcohol: 'Fullbar',
		//		Phone: '305-000-0000',
		//		WebsiteUrl: 'http://www.google.com',
		//		GoodForDancing: true,
		//		Details: '<h1><strong>Hello</strong> World</h1>',
		//		Amenities: 'FULL-TIME DOORMAN,CITY VIEWS,SMOKING area, large dance floor, downtown view, ocean view, light effects, happy hour',
		//		AmenitiesPhrase: 'These are just some of the cool things you can experience in this club',

		//		WeekHours: {
		//			Monday: {
		//				Day: 1
		//			},
		//			Tuesday: {
		//				Day: 2
		//			},
		//			Wednesday: {
		//				Day: 3
		//			},
		//			Thursday: {
		//				Day: 4
		//			},
		//			Friday: {
		//				Day: 5
		//			},
		//			Saturday: {
		//				Day: 6
		//			},
		//			Sunday: {
		//				Day: 0
		//			}
		//		},

		//		Images: [
		//			{ "BinaryDataId": 462, "FileName": 'a', "ContentType": "image/jpeg" },
		//			{ "BinaryDataId": 463, "FileName": 'a', "ContentType": "image/jpeg" },
		//			{ "BinaryDataId": 464, "FileName": 'doral.jpg', "ContentType": "image/jpeg" }
		//		],

		//		Address: {
		//			Street: '900 Ocean Dr',
		//			City: 'Miami',
		//			State: 'FL',
		//			ZipCode: '33139'
		//		}
		//	};
		//};

		// $scope.model = newBusiness();

		$scope.toggleRenderingMode = function () {
			$scope.renderingMode = $scope.renderingMode === vip.renderingModes.edit ? vip.renderingModes.read : vip.renderingModes.edit;
		};
	}]);
});