define(['vip/js/vip', 'jquery', 'sweet-alert'], function (vip, jQuery, swal) {
	'use strict';

	vip.controller('vip.addBusinessController', ['$scope', function ($scope) {
		$scope.renderingMode = vip.renderingModes.read;

		var newBusiness = function () {
			return {
				Name: 'Dance by the Ocean Nightclub',
				Phrase: 'People, food, music and much more...',
				Parking: 'Street/Valet',
				Ambiance: 'Upscale Trendy',
				Alcohol: 'Fullbar',
				Phone: '305-000-0000',
				WebsiteUrl: 'http://www.google.com',
				GoodForDancing: true,
				Details: '<h1><strong>Hello</strong> World</h1>',
				Amenities: 'FULL-TIME DOORMAN,CITY VIEWS,SMOKING area, large dance floor, downtown view, ocean view, light effects, happy hour',
				AmenitiesPhrase: 'These are just some of the cool things you can experience in this club',

				WeekHours: {
					Monday: {
						Day: 1
					},
					Tuesday: {
						Day: 2
					},
					Wednesday: {
						Day: 3
					},
					Thursday: {
						Day: 4
					},
					Friday: {
						Day: 5
					},
					Saturday: {
						Day: 6
					},
					Sunday: {
						Day: 0
					}
				},

				Images: [
					{ "BinaryDataId": 462, "FileName": 'a', "ContentType": "image/jpeg" },
					{ "BinaryDataId": 463, "FileName": 'a', "ContentType": "image/jpeg" },
					{ "BinaryDataId": 464, "FileName": 'doral.jpg', "ContentType": "image/jpeg" }
				],

				Address: {
					Street: '900 Ocean Dr',
					City: 'Miami',
					State: 'FL',
					ZipCode: '33139'
				}
			};
		};

		$scope.model = newBusiness();

		$scope.toggleRenderingMode = function () {
			$scope.renderingMode = $scope.renderingMode === vip.renderingModes.edit ? vip.renderingModes.read : vip.renderingModes.edit;
		};
	}]);
});