import { Aurelia, LogManager, ViewLocator, Origin } from 'aurelia-framework';

import environment from './environment';
import { CustomLogAppender } from './core/custom-log-appender';

// Configure Bluebird Promises.
(<any>Promise).config({
  warnings: {
    wForgottenReturn: false
  }
});

LogManager.addAppender(new CustomLogAppender());
LogManager.setLevel(LogManager.logLevel.debug);

export function configure(aurelia: Aurelia) {
  aurelia.use
    .standardConfiguration()
    .feature('resources');

  ViewLocator.prototype.convertOriginToViewUrl = (origin: Origin): string => {
    let moduleId = origin.moduleId;

    let isVm = moduleId.endsWith('.view-model');

    let id = isVm ? moduleId.split('/')[1].split('.')[0] : moduleId;

    return isVm ? `views/${id}.view.html` : `${id}.html`;
  };

  if (environment.testing) {
    aurelia.use.plugin('aurelia-testing');
  }

  aurelia.start().then(() => aurelia.setRoot());
}
