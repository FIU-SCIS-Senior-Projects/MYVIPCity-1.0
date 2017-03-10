define([
	'require',
	'angular',
	'vip/js/vip',
	// config
	'vip/js/config/vipConfig',
	'vip/js/config/vipRouteConfig',
	// controllers
	'vip/js/controllers/addBusinessController',
	'vip/js/controllers/businessPromotersController',
	'vip/js/controllers/clubListController',
	'vip/js/controllers/forgotPasswordFormController',
	'vip/js/controllers/loginFormController',
	'vip/js/controllers/promoterProfileController',
	'vip/js/controllers/registerFormController',
	'vip/js/controllers/viewBusinessController',
	// directives
	'vip/js/directives/controls/vip-address',
	'vip/js/directives/controls/vip-checkbox',
	'vip/js/directives/controls/vip-day-hours',
	'vip/js/directives/controls/vip-existing-promoters',
	'vip/js/directives/controls/vip-images-control',
	'vip/js/directives/controls/vip-images-gallery',
	'vip/js/directives/controls/vip-images-upload',
	'vip/js/directives/controls/vip-link',
	'vip/js/directives/controls/vip-profile-picture',
	'vip/js/directives/controls/vip-promoter-name',
	'vip/js/directives/controls/vip-rating',
	'vip/js/directives/controls/vip-tags',
	'vip/js/directives/controls/vip-textbox',
	'vip/js/directives/controls/vip-time-picker',
	'vip/js/directives/controls/vip-week-hours',
	'vip/js/directives/controls/vip-wysiwyg',
	'vip/js/directives/vip-auto-grow-input',
	'vip/js/directives/vip-match',
	'vip/js/directives/vip-mvc-antiforgery-token',
	'vip/js/directives/vip-navigation-menu',
	'vip/js/directives/vip-new-promoters',
	// services
	'vip/js/services/factories/business',
	'vip/js/services/vipColorsService',
	'vip/js/services/vipControlRenderingService',
	'vip/js/services/vipFactoryService',
	'vip/js/services/vipNavigationService',
	'vip/js/services/vipServerErrorProcessorService',
	'vip/js/services/vipUtilsService'
], function (require, angular, vip) {
	return vip;
});

