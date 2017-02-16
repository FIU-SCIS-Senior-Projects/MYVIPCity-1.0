define([
	'require',
	'angular',
	'vip/js/vip',
	// config
	'vip/js/config/vipConfig',
	'vip/js/config/vipRouteConfig',
	// controllers
	'vip/js/controllers/addBusinessController',
	'vip/js/controllers/forgotPasswordFormController',
	'vip/js/controllers/loginFormController',
	'vip/js/controllers/registerFormController',
	// directives
	'vip/js/directives/controls/vip-textbox',
	'vip/js/directives/vip-match',
	'vip/js/directives/vip-mvc-antiforgery-token',
	'vip/js/directives/vip-navigation-menu',
	// services
	'vip/js/services/vipServerErrorProcessorService'
], function (require, angular, vip) {
	return vip;
});