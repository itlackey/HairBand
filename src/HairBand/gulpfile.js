/// <binding AfterBuild='copy, copy_themes, copy_pages, copy_riffs' Clean='clean, clean_themes, clean_pages, copy_site_data, clean_riffs' />
var gulp = require("gulp"),
  rimraf = require("rimraf"),
  less = require("gulp-less"),
  path = require("path"),
  plumber = require('gulp-plumber'),
  fs = require("fs");

eval("var project = " + fs.readFileSync("./project.json"));

var paths = {
    bower: "./bower_components/",
    lib: "./" + project.webroot + "/lib/",
    themes: "./" + project.webroot + "/themes/",
    data_root: "./" + project.webroot + "/app_data/",
    pages: "./" + project.webroot + "/app_data/_pages",
    drafts: "./" + project.webroot + "/app_data/_drafts",
    posts: "./" + project.webroot + "/app_data/_posts",
    secure: "./" + project.webroot + "/app_data/_secure",
    riffs: "./" + project.webroot + "/app_data/_riffs"
};

gulp.task("clean", function (cb) {
    rimraf(paths.lib, cb);
});

gulp.task("copy", ["clean"], function () {
    var bower = {
        "bootstrap": "bootstrap/dist/**/*.{js,map,css,ttf,svg,woff,eot}",
        "bootstrap-touch-carousel": "bootstrap-touch-carousel/dist/**/*.{js,css}",
        "hammer.js": "hammer.js/hammer*.{js,map}",
        "jquery": "jquery/jquery*.{js,map}",
        "jquery-validation": "jquery-validation/jquery.validate.js",
        "jquery-validation-unobtrusive": "jquery-validation-unobtrusive/jquery.validate.unobtrusive.js"
    }

    for (var destinationDir in bower) {
        gulp.src(paths.bower + bower[destinationDir])
          .pipe(gulp.dest(paths.lib + destinationDir));
    }
});

gulp.task("clean_themes", function (cb) {
    rimraf(paths.themes, cb);
});

//gulp.task('less', function () {

//    return gulp.src('./Themes/**/*.less')
//        .pipe(plumber())
//        .pipe(less({
//              paths: [path.join(__dirname, 'less', 'includes')]
//        }))
//        .pipe(gulp.dest('./wwwroot/themes'));
//});


gulp.task("copy_themes", function () {
    
    gulp.src('./Themes/**/*.less')
        .pipe(plumber())
        .pipe(less({
            compress: true,
            paths: [path.join(__dirname, 'less', 'includes')]
        }))
        .pipe(gulp.dest('./themes'));

    gulp.src("./themes/**")
        .pipe(gulp.dest(paths.themes));
});


gulp.task("clean_riffs", function (cb) {
    rimraf(paths.riffs, cb);
});

gulp.task("copy_riffs", function () {
    gulp.src("./app_data/_riffs/**")
        .pipe(gulp.dest(paths.riffs));

});

gulp.task("clean_pages", function (cb) {
    rimraf(paths.pages, function () { });
    rimraf(paths.drafts, function () { });
    rimraf(paths.posts, cb);       
});

gulp.task("copy_pages", function () {
    gulp.src("./app_data/_pages/**")
        .pipe(gulp.dest(paths.pages));

    gulp.src("./app_data/_drafts/**")
      .pipe(gulp.dest(paths.drafts));

    gulp.src("./app_data/_posts/**")
        .pipe(gulp.dest(paths.posts));
});

gulp.task("clean_site_data", function (cb) {
    rimraf(paths.data_root + "_config.yml", function () { });
    rimraf(paths.secure, cb);
});

gulp.task("copy_site_data", function () {
    gulp.src("./app_data/_config.yml")
        .pipe(gulp.dest(paths.data_root));
});

//gulp.task("clean_app_data", function (cb) {
//    rimraf(paths.data_root, cb);
//});

//gulp.task("copy_app_data", function () {
//    gulp.src("./app_data/**")
//        .pipe(gulp.dest(paths.data_root));
//});