define(['vip/js/vip', 'jquery', 'sweet-alert'], function (vip, jQuery, swal) {
	'use strict';

	vip.controller('vip.addBusinessController', ['$scope', function ($scope) {
		$scope.renderingMode = vip.renderingModes.edit;
		$scope.Name = 'Dance by the Ocean Nightclub';

		$scope.toggleRenderingMode = function () {
			$scope.renderingMode = $scope.renderingMode === vip.renderingModes.edit ? vip.renderingModes.read : vip.renderingModes.edit;
		};
	}]);
});