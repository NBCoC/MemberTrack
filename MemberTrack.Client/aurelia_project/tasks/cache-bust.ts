import * as gulp from 'gulp';
import * as replace from 'gulp-replace';

const timestamp = new Date().getTime();

function cacheBustVendor() {
  return gulp.src(`./scripts/vendor-bundle.js`)
    .pipe(replace(/\.\.\/\w+\/app-bundle/g, `../scripts/app-bundle.js?v=${timestamp}`))
    .pipe(gulp.dest(`./scripts`));
}

function cacheBustIndex() {
  return gulp.src('./src/index.html')
    .pipe(replace(`scripts/vendor-bundle.js`, `scripts/vendor-bundle.js?v=${timestamp}`))
    .pipe(gulp.dest('./'));
}

export default gulp.series(cacheBustVendor, cacheBustIndex);
