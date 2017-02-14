"use strict";

// dependencies
var gulp = require('gulp');
var uglify = require('gulp-uglify');
var rename = require('gulp-rename');

// task to minify app.js file
gulp.task('appMinify', function() {
	return gulp
		.src('Scripts/app/app.js')
		.pipe(rename('app.min.js'))
		.pipe(uglify())
		.pipe(gulp.dest('Scripts/app'));
});

