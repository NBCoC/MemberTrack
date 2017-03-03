import * as gulp from 'gulp';
import clean from './clean';
import transpile from './transpile';
import processMarkup from './process-markup';
import processCSS from './process-css';
import prepareMaterialIcons from './prepare-material-icons';
import { build } from 'aurelia-cli';
import * as project from '../aurelia.json';

export default gulp.series(
  readProjectConfiguration,
  gulp.parallel(
    clean,
    transpile,
    processMarkup,
    processCSS,
    prepareMaterialIcons
  ),
  writeBundles
);

function readProjectConfiguration() {
  return build.src(project);
}

function writeBundles() {
  return build.dest();
}
