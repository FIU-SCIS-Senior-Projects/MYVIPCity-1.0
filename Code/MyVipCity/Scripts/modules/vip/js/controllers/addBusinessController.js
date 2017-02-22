define(['vip/js/vip', 'jquery', 'sweet-alert'], function (vip, jQuery, swal) {
	'use strict';

	vip.controller('vip.addBusinessController', ['$scope', function ($scope) {
		$scope.renderingMode = vip.renderingModes.edit;

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
				Amenities: 'FULL-TIME DOORMAN,RIVER VIEWS,COMMON ,ROOF DECK,CITY VIEWS,GYM,SAUNA,OPEN VIEWS,BASKETBALL COURT,POOL',
				AmenitiesPhrase: 'These are just some of the cool things you can experience in this club',

				WeekHours: {
					Monday: {
						Day: 1,
						OpenTime: '2017-02-21T10:00:00',
						CloseTime: new Date()
					},
					Tuesday: {
						Day: 2,
						OpenTime: '2017-02-21T10:00:00',
						CloseTime: new Date()
					},
					Wednesday: {
						Day: 3,
						OpenTime: '2017-02-21T10:00:00',
						CloseTime: new Date()
					},
					Thursday: {
						Day: 4,
						OpenTime: '2017-02-21T10:00:00',
						CloseTime: new Date()
					},
					Friday: {
						Day: 5,
						OpenTime: '2017-02-21T10:00:00',
						CloseTime: new Date()
					},
					Saturday: {
						Day: 6,
						OpenTime: '2017-02-21T10:00:00',
						CloseTime: new Date()
					},
					Sunday: {
						Day: 0,
						OpenTime: '2017-02-21T10:00:00',
						CloseTime: new Date()
					}
				}
			};
		};

		$scope.model = newBusiness();

		$scope.toggleRenderingMode = function () {
			$scope.renderingMode = $scope.renderingMode === vip.renderingModes.edit ? vip.renderingModes.read : vip.renderingModes.edit;
		};
	}]);
});