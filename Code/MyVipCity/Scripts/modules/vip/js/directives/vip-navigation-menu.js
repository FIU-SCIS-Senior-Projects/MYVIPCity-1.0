define(['vip/js/vip'], function (vip) {
	'use strict';

	vip.directive('vipNavigationMenu', ['vipConfig', function (vipConfig) {
		return {
			restrict: 'AEC',

			replace: true,

			template:
				'<div>' +
					'<div class="navigation-trigger visible-xs visible-sm" data-rmd-action="block-open" data-rmd-target=".navigation">' +
                        '<i class="zmdi zmdi-menu"></i>' +
                    '</div>' +
					'<ul class="navigation">' +
						'<li class="visible-xs visible-sm"><a class="navigation__close" data-rmd-action="navigation-close" href=""><i class="zmdi zmdi-long-arrow-right"></i></a></li>' +
						'<li ng-repeat="menuItem in menuItems" class="navigation__dropdown">' +
                            '<a href="{{menuItem.Path}}">{{menuItem.Title}}</a>' +
                            '<ul class="navigation__drop-menu">' +
                                '<li ng-repeat="submenuItem in menuItem.Submenu">' +
									'<a href="{{submenuItem.Path}}">{{submenuItem.Title}}</a>' +
								'</li>' +
                            '</ul>' +
                        '</li>' +
					'</ul>' +
				'</div>',
			controller: ['$scope', function ($scope) {
				$scope.menuItems = vipConfig.Menu;
			}]
		};
	}]);
});