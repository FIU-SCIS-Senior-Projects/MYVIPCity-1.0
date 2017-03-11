define(['vip/js/vip'], function (vip) {
	'use strict';

	vip.directive('vipHostsCard', [function () {
		return {
			restrict: 'AC',

			replace: true,

			template:
				'<div class="card">' +
					'<div class="card__header">' +
						'<h2>VIP Hosts</h2>' +
						'<small>These VIP hosts will make you have the best time</small>' +
					'</div>' +
					'<div class="list-group">' +
						'<div vip-existing-promoters vip-business-id="{{model.Id}}">' +
						'</div>' +
						'<div ng-controller="vip.businessPromotersController">' +
							'<!--List pending invitations-->' +
							'<div class="vip-promoters-pending" ng-show="pendingInvitations.length && renderingMode == 2">' +
								'<h2>Pending invitations</h2>' +
								'<ul>' +
									'<li ng-repeat="invitation in pendingInvitations">' +
										'<span class="vip-promoter-pending__name">{{invitation.Name}}</span>' +
										'<span class="vip-promoter-pending__email"><a href="mailto:{{invitation.Email}}">({{invitation.Email}})</a></span>' +
										'<i class="zmdi zmdi-email" title="Resend invitation" ng-click="resendInvitation(invitation)"></i>' +
										'<i class="zmdi zmdi-delete" title="Delete" ng-click="deleteInvitation(invitation)"></i>' +
									'</li>' +
								'</ul>' +
							'</div>' +
							'<!--New invitations directive-->' +
							'<div vip-new-promoters ng-model="newPromoterInvitations" ng-show="renderingMode == 2">' +
							'</div>' +
						'</div>' +
					'</div>' +
				'</div>'
		};
	}]);
});