"use strict";

// dependencies
var gulp = require('gulp');
var uglify = require('gulp-uglify');
var rename = require('gulp-rename');
var less = require('gulp-less');
var minifyCSS = require('gulp-csso');

// task to minify app.js file
gulp.task('appMinify', function() {
	return gulp
		.src('Scripts/app/app.js')
		.pipe(rename('app.min.js'))
		.pipe(uglify())
		.pipe(gulp.dest('Scripts/app'));
});

// taks to compile .less files into .css
gulp.task('css', function() {
	return gulp
		.src('Content/less/*.less')
		.pipe(less())
		.pipe(gulp.dest('Content/css'))
		.pipe(minifyCSS())
		.pipe(rename({suffix: '.min'}))
		.pipe(gulp.dest('Content/css'));
});

