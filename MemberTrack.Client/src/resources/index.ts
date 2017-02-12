import { FrameworkConfiguration } from 'aurelia-framework';

export function configure(config: FrameworkConfiguration) {
  config.globalResources([
    './attributes/toggle-drawer',
    './attributes/mdl',
    './value-converters/sort',
    './value-converters/group',
    './value-converters/filter',
    './value-converters/date-format',
    './elements/snackbar'
  ]);
}
