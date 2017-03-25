﻿define(['vip/js/vip', 'angular'], function (vip, angular) {
	'use strict';

	vip.factory('vipColorsService', ['vipUtilsService', function (vipUtilsService) {
		var colors = {
			Red:
			{
				"300":
					"#e57373",
				"400":
					"#ef5350",
				"500":
					"#f44336",
				"600":
					"#e53935",
				"700":
					"#d32f2f",
				"800":
					"#c62828",
				"900":
					"#b71c1c",
				"A100":
					"#ff8a80",
				"A200":
					"#ff5252",
				"A400":
					"#ff1744",
				"A700":
					"#d50000"
			},
			Pink: {
				"300":
					"#f06292",
				"400":
					"#ec407a",
				"500":
					"#e91e63",
				"600":
					"#d81b60",
				"700":
					"#c2185b",
				"800":
					"#ad1457",
				"900":
					"#880e4f",
				"A100":
					"#ff80ab",
				"A200":
					"#ff4081",
				"A400":
					"#f50057",
				"A700":
					"#c51162"
			},
			Purple: {
				"300":
					"#ba68c8",
				"400":
					"#ab47bc",
				"500":
					"#9c27b0",
				"600":
					"#8e24aa",
				"700":
					"#7b1fa2",
				"800":
					"#6a1b9a",
				"900":
					"#4a148c",
				"A100":
					"#ea80fc",
				"A200":
					"#e040fb",
				"A400":
					"#d500f9",
				"A700":
					"#aa00ff"
			},
			'Deep-Purple': {
				"200":
					"#b39ddb",
				"300":
					"#9575cd",
				"400":
					"#7e57c2",
				"500":
					"#673ab7",
				"600":
					"#5e35b1",
				"700":
					"#512da8",
				"800":
					"#4527a0",
				"900":
					"#311b92",
				"A100":
					"#b388ff",
				"A200":
					"#7c4dff",
				"A400":
					"#651fff",
				"A700":
					"#6200ea"
			},
			Indigo: {
				"300":
					"#7986cb",
				"400":
					"#5c6bc0",
				"500":
					"#3f51b5",
				"600":
					"#3949ab",
				"700":
					"#303f9f",
				"800":
					"#283593",
				"900":
					"#1a237e",
				"A100":
					"#8c9eff",
				"A200":
					"#536dfe",
				"A400":
					"#3d5afe",
				"A700":
					"#304ffe"
			},
			Blue: {
				"300":
					"#64b5f6",
				"400":
					"#42a5f5",
				"500":
					"#2196f3",
				"600":
					"#1e88e5",
				"700":
					"#1976d2",
				"800":
					"#1565c0",
				"900":
					"#0d47a1",
				"A100":
					"#82b1ff",
				"A200":
					"#448aff",
				"A400":
					"#2979ff",
				"A700":
					"#2962ff"
			},
			'Light-Blue': {
				"300":
					"#4fc3f7",
				"400":
					"#29b6f6",
				"500":
					"#03a9f4",
				"600":
					"#039be5",
				"700":
					"#0288d1",
				"800":
					"#0277bd",
				"900":
					"#01579b",
				"A100":
					"#80d8ff",
				"A200":
					"#40c4ff",
				"A400":
					"#00b0ff",
				"A700":
					"#0091ea"
			},
			Cyan: {
				"300":
					"#4dd0e1",
				"400":
					"#26c6da",
				"500":
					"#00bcd4",
				"600":
					"#00acc1",
				"700":
					"#0097a7",
				"800":
					"#00838f",
				"900":
					"#006064"
			},
			Teal: {
				"300":
					"#4db6ac",
				"400":
					"#26a69a",
				"500":
					"#009688",
				"600":
					"#00897b",
				"700":
					"#00796b",
				"800":
					"#00695c",
				"900":
					"#004d40"
			},
			Green: {
				"300":
					"#81c784",
				"400":
					"#66bb6a",
				"500":
					"#4caf50",
				"600":
					"#43a047",
				"700":
					"#388e3c",
				"800":
					"#2e7d32",
				"900":
					"#1b5e20"
			},
			"Light-Green":
			{
				"300":
					"#aed581",
				"400":
					"#9ccc65",
				"500":
					"#8bc34a",
				"600":
					"#7cb342",
				"700":
					"#689f38",
				"800":
					"#558b2f",
				"900":
					"#33691e"
			},
			Lime: {
				"600":
					"#c0ca33",
				"700":
					"#afb42b",
				"800":
					"#9e9d24",
				"900":
					"#827717"
			},
			Yellow: {
				"700":
					"#fbc02d",
				"800":
					"#f9a825",
				"900":
					"#f57f17"
			},
			Amber: {
				"600":
					"#ffb300",
				"700":
					"#ffa000",
				"800":
					"#ff8f00",
				"900":
					"#ff6f00",
				"A100":
					"#ffe57f",
				"A200":
					"#ffd740",
				"A400":
					"#ffc400",
				"A700":
					"#ffab00"
			},
			Orange: {
				"600":
					"#fb8c00",
				"700":
					"#f57c00",
				"800":
					"#ef6c00",
				"900":
					"#e65100",
				"A100":
					"#ffd180",
				"A200":
					"#ffab40",
				"A400":
					"#ff9100",
				"A700":
					"#ff6d00"
			},
			"Deep-Orange":
			{
				"300":
					"#ff8a65",
				"400":
					"#ff7043",
				"500":
					"#ff5722",
				"600":
					"#f4511e",
				"700":
					"#e64a19",
				"800":
					"#d84315",
				"900":
					"#bf360c",
				"A100":
					"#ff9e80",
				"A200":
					"#ff6e40",
				"A400":
					"#ff3d00",
				"A700":
					"#dd2c00"
			},
			Brown: {
				"300":
					"#a1887f",
				"400":
					"#8d6e63",
				"500":
					"#795548",
				"600":
					"#6d4c41",
				"700":
					"#5d4037",
				"800":
					"#4e342e",
				"900":
					"#3e2723"
			},
			Grey: {
				"500":
					"#9e9e9e",
				"600":
					"#757575",
				"700":
					"#616161",
				"800":
					"#424242",
				"900":
					"#212121"
			},
			"Blue-Grey":
			{
				"300":
					"#90a4ae",
				"400":
					"#78909c",
				"500":
					"#607d8b",
				"600":
					"#546e7a",
				"700":
					"#455a64",
				"800":
					"#37474f",
				"900":
					"#263238"
			}
		};

		var lightColors = [
				'#ffebee',
				// '#fce4ec',
				// '#f3e5f5',
				// '#ede7f6',
				'#e8eaf6',
				'#e3f2fd',
				'#e1f5fe',
				'#e0f7fa',
				'#e0f2f1',
				'#e8f5e9',
				'#f1f8e9',
				'#f9fbe7',
				'#fffde7',
				'#fff8e1',
				'#fff3e0',
				'#fbe9e7',
				'#efebe9',
				'#fafafa',
				'#eceff1'
		];

		return {
			getColorsName: function () {
				var colorNames = [];
				angular.forEach(colors, function (value, key) {
					if (colors.hasOwnProperty(key))
						colorNames.push(key);
				});
				return colorNames;
			},
			getColorShadesName: function (colorName) {
				var shadesNames = [];
				angular.forEach(colors[colorName], function (value, key) {
					if (colors[colorName].hasOwnProperty(key))
						shadesNames.push(key);
				});
				return shadesNames;
			},
			getRandomBackgroundColorClass: function (colorName) {
				if (!colorName) {
					var colors = this.getColorsName;
					colorName = colors[vipUtilsService.getRandomIntInclusive(0, colors.length - 1)];
				}

				var shades = this.getColorShadesName(colorName);
				var shade = shades[vipUtilsService.getRandomIntInclusive(0, shades.length - 1)];

				return ('mdc-bg-' + colorName.toLowerCase() + '-' + shade);
			},
			getRandomLightColor: function() {
				return vipUtilsService.getRandomFromArray(lightColors);
			},
			hexToRGB: function (hex, alpha) {
				var r = parseInt(hex.slice(1, 3), 16),
					g = parseInt(hex.slice(3, 5), 16),
					b = parseInt(hex.slice(5, 7), 16);

				if (alpha) {
					return "rgba(" + r + ", " + g + ", " + b + ", " + alpha + ")";
				} else {
					return "rgb(" + r + ", " + g + ", " + b + ")";
				}
			}
		};
	}]);
});