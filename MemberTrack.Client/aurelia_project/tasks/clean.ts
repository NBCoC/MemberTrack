import * as del from 'del';
import * as project from '../aurelia.json';
import environment from '../../src/environment';

export default function clean() {
    if (environment.debug) {
        return Promise.resolve(true);
    }

    const dir = project.platform.output;

    return del([`${dir}/*`, `${dir}/material-icons/**`, `!${dir}/material-icons`]).then(paths => {
        console.log('Deleted files and folders:\n', paths.join('\n'));
    });
}