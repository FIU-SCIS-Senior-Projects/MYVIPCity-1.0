define(['vip/js/vip', 'angular'], function (vip, angular) {
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
			},

			addFirstPictureUrl: function(business) {
				var array = angular.isArray(business) ? business : [business];

				angular.forEach(array, function (b) {
					// sort the pictures by Index and take the first one
					b.Pictures.sort(function (a, b) {
						return a.Index - b.Index;
					});
					b.firstPictureUrl = 'api/Pictures/' + b.Pictures[0].BinaryDataId;
				});

				return business;
			}
		};
	}]);
});