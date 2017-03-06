import * as gulp from 'gulp';
import * as project from '../aurelia.json';

export default function prepareMaterialIcons() {
    const source = 'src/styles';

    return gulp.src(`${source}/splash.css`)
        .pipe(gulp.dest(`${project.platform.output}`));
}