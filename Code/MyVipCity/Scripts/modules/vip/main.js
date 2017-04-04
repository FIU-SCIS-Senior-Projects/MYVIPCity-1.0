﻿define([
	'require',
	'angular',
	'vip/js/vip',
	// config
	'vip/js/config/vipConfig',
	'vip/js/config/vipRouteConfig',
	// controllers
	'vip/js/controllers/acceptDeclineAttendingRequestController',
	'vip/js/controllers/addBusinessController',
	'vip/js/controllers/businessPromotersController',
	'vip/js/controllers/clubListController',
	'vip/js/controllers/forgotPasswordFormController',
	'vip/js/controllers/loginFormController',
	'vip/js/controllers/mainController',
	'vip/js/controllers/promoterProfileController',
	'vip/js/controllers/registerFormController',
	'vip/js/controllers/viewBusinessController',
	'vip/js/controllers/wantToGoFormController',
	// directives
	'vip/js/directives/controls/vip-address',
	'vip/js/directives/controls/vip-checkbox',
	'vip/js/directives/controls/vip-datepicker',
	'vip/js/directives/controls/vip-day-hours',
	'vip/js/directives/controls/vip-existing-promoters',
	'vip/js/directives/controls/vip-images-control',
	'vip/js/directives/controls/vip-images-gallery',
	'vip/js/directives/controls/vip-images-upload',
	'vip/js/directives/controls/vip-link',
	'vip/js/directives/controls/vip-numeric-select',
	'vip/js/directives/controls/vip-posts',
	'vip/js/directives/controls/vip-profile-picture',
	'vip/js/directives/controls/vip-promoter-dropdown',
	'vip/js/directives/controls/vip-promoter-name',
	'vip/js/directives/controls/vip-promoter-picture',
	'vip/js/directives/controls/vip-rating',
	'vip/js/directives/controls/vip-tags',
	'vip/js/directives/controls/vip-textarea',
	'vip/js/directives/controls/vip-textbox',
	'vip/js/directives/controls/vip-time-picker',
	'vip/js/directives/controls/vip-video-control',
	'vip/js/directives/controls/vip-video',
	'vip/js/directives/controls/vip-week-hours',
	'vip/js/directives/controls/vip-wysiwyg',
	'vip/js/directives/validators/vip-non-empty-array',
	'vip/js/directives/vip-auto-grow-input',
	'vip/js/directives/vip-hosts-card',
	'vip/js/directives/vip-match',
	'vip/js/directives/vip-mvc-antiforgery-token',
	'vip/js/directives/vip-navigation-menu',
	'vip/js/directives/vip-new-promoters',
	'vip/js/directives/vip-random-bg-color',
	// services
	'vip/js/services/factories/business-posts-manager',
	'vip/js/services/factories/business',
	'vip/js/services/factories/comment-post',
	'vip/js/services/factories/picture-posts',
	'vip/js/services/factories/promoter-posts-manager',
	'vip/js/services/factories/video-posts',
	'vip/js/services/vipColorsService',
	'vip/js/services/vipControlRenderingService',
	'vip/js/services/vipFactoryService',
	'vip/js/services/vipNavigationService',
	'vip/js/services/vipPageLoaderService',
	'vip/js/services/vipServerErrorProcessorService',
	'vip/js/services/vipUserService',
	'vip/js/services/vipUtilsService',
	'vip/js/services/vipVideoService'
], function (require, angular, vip) {
	return vip;
});

