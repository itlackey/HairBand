/// <binding AfterBuild='copy, copy_themes' Clean='clean, clean_themes' />
var gulp = require("gulp"),
  rimraf = require("rimraf"),
  fs = require("fs");

eval("var project = " + fs.readFileSync("./project.json"));

var paths = {
    bower: "./bower_components/",
    lib: "./" + project.webroot + "/lib/",
    themes: "./" + project.webroot + "/themes/",
    data_root: "./" + project.webroot + "/app_data/",
    pages: "./" + project.webroot + "/_pages/",
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

gulp.task("copy_themes", function () {
    gulp.src("./themes/**")
        .pipe(gulp.dest(paths.themes));
});

gulp.task("clean_site_data", function (cb) {
    rimraf(paths.data_root, cb);
});

gulp.task("copy_site_data", function () {
    gulp.src("./app_data/**")
        .pipe(gulp.dest(paths.data_root));
});