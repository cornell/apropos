var del = require('del');
var fs = require('fs-extra');
var path = require('path');
var gulp = require('gulp');
var gulpif = require('gulp-if');
var debug = require('gulp-debug');
var sass = require('gulp-sass');
var autoprefixer = require("gulp-autoprefixer");
var csso = require('gulp-csso');
var sourcemaps = require('gulp-sourcemaps');
var sassdoc = require('sassdoc');
var rename = require('gulp-rename');
var wkhtmltopdf = require('wkhtmltopdf');
var tap = require('gulp-tap');

var inputSassFolder = 'assets/sass/';
var autoprefixerOptions = {
    browsers: ['last 2 versions'],
    cascade: false
};

gulp.task("del", function () {
    del(['wwwroot/css/app.css'
    ]);
});

gulp.task("sass", function () {

    gulp.src(inputSassFolder + 'app.scss')

        // init sourcemaps
        .pipe(sourcemaps.init())

        // compile to sass
        .pipe(sass({
            errLogToConsole: true,
            outputStyle: 'expanded'
        })).on('error', sass.logError)

        // add vendor prefixes
        .pipe(autoprefixer(autoprefixerOptions))

        // write sourcemaps
        .pipe(sourcemaps.write('./wwwroot/css/maps'))

        .pipe(gulp.dest('wwwroot/css'));

    gulp.src(inputSassFolder + 'backend-app.scss')

        // init sourcemaps
        .pipe(sourcemaps.init())

        // compile to sass
        .pipe(sass({
            errLogToConsole: true,
            outputStyle: 'expanded'
        })).on('error', sass.logError)

        // add vendor prefixes
        .pipe(autoprefixer(autoprefixerOptions))

        // write sourcemaps
        .pipe(sourcemaps.write('./wwwroot/css/maps'))

        .pipe(gulp.dest('wwwroot/css'));

    gulp.src(inputSassFolder + 'contrat.scss')

        // compile to sass
        .pipe(sass({
            errLogToConsole: true,
            outputStyle: 'expanded'
        })).on('error', sass.logError)

        // add vendor prefixes
        .pipe(autoprefixer(autoprefixerOptions))

        .pipe(gulp.dest('wwwroot/css'));
});

gulp.task('sassdoc', function () {
    return gulp
      .src(inputSassFile)
      .pipe(sassdoc())
      .resume();
});

gulp.task('watch', function () {
    return gulp
      // Watch the input folder for change,
      // and run `sass` task when something happens
      .watch('assets/sass/**/*.scss', ['sass'])
      // When there is a change,
      // log a message in the console
      .on('change', function (event) {
          console.log('File ' + event.path + ' was ' + event.type + ', running tasks...');
      });
});

gulp.task("copy", function () {

    gulp.src('assets/font/**')
    .pipe(gulp.dest('wwwroot/font'));
});

gulp.task("htmltopdf", function () {

    var i = 0;
    gulp
        .src('wwwroot/formation/**/*.html')
        .pipe(tap(function (file, e) {
            var filename = path.basename(file.path).replace('.html', '.pdf');            
            wkhtmltopdf(file.contents, { output: path.dirname(file.path) + '/' + filename });
        }))
        .pipe(gulp.dest('wwwroot/formation'));
});

gulp.task('prod', ['sassdoc'], function () {
    return gulp
      .src(inputSassFile)
      .pipe(sass({
          outputStyle: 'compressed'
      }))
      .pipe(autoprefixer(autoprefixerOptions))
      .pipe(gulp.dest(output));
});

gulp.task('default', ['sass', 'watch'], function () {
    
});