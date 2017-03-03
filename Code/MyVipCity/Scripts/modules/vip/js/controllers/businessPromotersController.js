define(['vip/js/vip'], function (vip) {
	'use strict';

	vip.controller('vip.businessPromotersController', ['$scope', '$http', function ($scope, $http) {
		$scope.newPromoters = [
			{
				Id: 0,
				Name: 'Angel',
				Email: 'a@a.com'
			},
			{
				Id: 0,
				Name: 'Dianet',
				Email: 'd@d.com'
			}
		];

	}]);
});