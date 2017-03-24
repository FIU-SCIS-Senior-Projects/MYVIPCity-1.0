define(['vip/js/vip', 'angular'], function (vip, angular) {
	'use strict';

	vip.factory('vipVideoService', [function () {

		return {
			getVideoElement: function (url) {
				var pattern1 = /(?:http?s?:\/\/)?(?:www\.)?(?:vimeo\.com)\/?(.+)/g;
				var pattern2 = /(?:http?s?:\/\/)?(?:www\.)?(?:youtube\.com|youtu\.be)\/(?:watch\?v=)?(.+)/g;
				var pattern3 = /(?:http?s?:\/\/)?(?:www\.)?(?:youtube\.com|youtu\.be)\/(?:w‌​atch\?v=|embed\/)?(.‌​+)/g;

				var videoElement, iframeStr;
				// check for VIMEO
				if (pattern1.test(url)) {
					iframeStr = '<iframe width="420" height="345" src="//player.vimeo.com/video/$1" frameborder="0" webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>';
					iframeStr = url.replace(pattern1, iframeStr);
				} else if (pattern2.test(url)) {
					iframeStr = '<iframe width="420" height="345" src="http://www.youtube.com/embed/$1" frameborder="0" allowfullscreen></iframe>';
					iframeStr = url.replace(pattern2, iframeStr);
				} else if (pattern3.test(url)) {
					iframeStr = '<iframe width="420" height="345" src="http://www.youtube.com/embed/$1" frameborder="0" allowfullscreen></iframe>';
					iframeStr = url.replace(pattern3, iframeStr);
				}

				if (iframeStr)
					return angular.element(iframeStr);

				return null;
			}
		};
	}]);
});