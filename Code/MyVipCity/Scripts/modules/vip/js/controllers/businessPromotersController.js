define(['vip/js/vip', 'angular', 'sweet-alert'], function (vip, angular, swal) {
	'use strict';

	vip.controller('vip.businessPromotersController', ['$scope', '$http', function ($scope, $http) {
		$scope.newPromoterInvitations = [];
		$scope.pendingInvitations = [];
		var friendlyId;

		var getPromoterPendingInvitations = function () {
			return $http.get('api/PromoterInvitation/GetPendingInvitations/' + friendlyId).then(function (response) {
				$scope.pendingInvitations = response.data;
				return response.data;
			});
		};

		var unwatch = $scope.$watch('model.FriendlyId', function (id) {
			if (id) {
				friendlyId = id;
				getPromoterPendingInvitations();
				unwatch();
			}
		});

		var showErrorPopup = function (title, errorMsg) {
			swal(title || 'Oops', errorMsg || 'An error has occurred', 'error');
		};

		var sendInvitationToPromoters = function (invitations, successMessage) {
			return $http.post('api/PromoterInvitation/SendInvitation', invitations).then(function () {
				$scope.newPromoterInvitations = [];
				swal('Success', successMessage || 'Invitation sent successfully', 'success');
			}, function () {
				showErrorPopup('Oops', 'An error has occurred sending the invitations');
			});
		};

		$scope.sendInvitation = function () {
			angular.forEach($scope.newPromoterInvitations, function (newPromoter) {
				newPromoter.Id = 0;
				newPromoter.ClubFriendlyId = friendlyId;
			});
			sendInvitationToPromoters($scope.newPromoterInvitations, 'An invitation email has been sent to each new promoter').then(function () {
				getPromoterPendingInvitations();
			});
		};

		$scope.deleteInvitation = function (invitation) {
			swal({
				type: 'warning',
				title: 'Delete Invitation?',
				text: 'If you delete this pending invitation the promoter will not be able to join the club. Are you sure you want to continue?',
				confirmButtonText: "Yes, delete it",
				showCancelButton: true
			}).then(function () {
				$http.delete('api/PromoterInvitation/DeleteInvitation/' + invitation.Id).then(function () {
					getPromoterPendingInvitations();
				}, function () {
					showErrorPopup();
				});
			}, angular.noop);
		};

		$scope.resendInvitation = function (invitation) {
			swal({
				type: 'question',
				title: 'Resend invitation?',
				text: 'Are you sure you want to resend and invitation to ' + invitation.Name + ' at ' + invitation.Email + '?',
				confirmButtonText: "Yes",
				showCancelButton: true
			}).then(function () {
				sendInvitationToPromoters([invitation]);
			}, angular.noop);
		};
	}]);
});