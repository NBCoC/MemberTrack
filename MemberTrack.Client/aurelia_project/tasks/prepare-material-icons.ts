import * as gulp from 'gulp';
import * as merge from 'merge-stream';
import * as changedInPlace from 'gulp-changed-in-place';
import * as project from '../aurelia.json';

export default function prepareMaterialIcons() {
  const source = 'node_modules/material-design-icons/iconfont';

  const taskCss = gulp.src(`${source}/material-icons.css`)
    .pipe(changedInPlace({ firstPass: true }))
    .pipe(gulp.dest(`${project.platform.output}/material-icons`));

  const taskFonts = gulp.src([
    `${source}/MaterialIcons-Regular.eot`,
    `${source}/MaterialIcons-Regular.ijmap`,
    `${source}/MaterialIcons-Regular.svg`,
    `${source}/MaterialIcons-Regular.ttf`,
    `${source}/MaterialIcons-Regular.woff`,
    `${source}/MaterialIcons-Regular.woff2`
  ])
    .pipe(changedInPlace({ firstPass: true }))
    .pipe(gulp.dest(`${project.platform.output}/material-icons`));

  return merge(taskCss, taskFonts);
}