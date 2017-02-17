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
				WebsiteUrl: 'http://www.google.com'
			};
		};

		$scope.model = newBusiness();

		$scope.toggleRenderingMode = function () {
			$scope.renderingMode = $scope.renderingMode === vip.renderingModes.edit ? vip.renderingModes.read : vip.renderingModes.edit;
		};
	}]);
});