define(['vip/js/vip'], function (vip) {
	'use strict';

	vip.factory('vipBusinessService', ['$q', '$http', function ($q, $http) {

		return {
			search: function(searchCriteria) {
				var deferred = $q.defer();

				var params = searchCriteria || {};
				
				$http.get('api/Business', {
					params: params
				}).then(function(response) {
					deferred.resolve(response.data);
				}, function(error) {
					deferred.reject(error);
				});

				return deferred.promise;
			}
		};
	}]);
});