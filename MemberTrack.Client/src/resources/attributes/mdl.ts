import { customAttribute } from 'aurelia-framework';

const MdlControls = {
    'button': { type: 'MaterialButton', classes: ['mdl-button', 'mdl-js-button'], rippleEffectSupport: true },
    'textfield': { type: 'MaterialTextfield', classes: ['mdl-textfield', 'mdl-js-textfield'], rippleEffectSupport: false },
    'layout': { type: 'MaterialLayout', classes: ['mdl-layout', 'mdl-js-layout'], rippleEffectSupport: false },
    'menu': { type: 'MaterialMenu', classes: ['mdl-menu', 'mdl-js-menu'], rippleEffectSupport: true },
    'data-table': { type: 'MaterialDataTable', classes: ['mdl-data-table', 'mdl-js-data-table'], rippleEffectSupport: true },
    'tabs': { type: 'MaterialTabs', classes: ['mdl-tabs', 'mdl-js-tabs'], rippleEffectSupport: true },
    'slider': { type: 'MaterialSlider', classes: ['mdl-slider', 'mdl-js-slider'], rippleEffectSupport: false },
    'tooltip': { type: 'MaterialTooltip', classes: ['mdl-tooltip'], rippleEffectSupport: false },
    'progress': { type: 'MaterialProgress', classes: ['mdl-progress', 'mdl-js-progress'], rippleEffectSupport: false },
    'spinner': { type: 'MaterialSpinner', classes: ['mdl-spinner', 'mdl-js-spinner'], rippleEffectSupport: false },
    'badge': { type: 'MaterialBadge', classes: [''], rippleEffectSupport: false },
    'switch': { type: 'MaterialSwitch', classes: ['mdl-switch', 'mdl-js-switch'], rippleEffectSupport: true },
    'radio': { type: 'MaterialRadio', classes: ['mdl-radio', 'mdl-js-radio'], rippleEffectSupport: true },
    'icon-toggle': { type: 'MaterialIconToggle', classes: ['mdl-icon-toggle', 'mdl-js-icon-toggle'], rippleEffectSupport: true },
    'checkbox': { type: 'MaterialCheckbox', classes: ['mdl-checkbox', 'mdl-js-checkbox'], rippleEffectSupport: true },
    'snackbar': { type: 'MaterialSnackbar', classes: ['mdl-snackbar', 'mdl-js-snackbar'], rippleEffectSupport: false },
    'selectfield': { type: 'MaterialSelectfield', classes: ['mdl-selectfield', 'mdl-js-selectfield'], rippleEffectSupport: false }
};

@customAttribute('mt-mdl')
export class MdlAttribute {
    private element: Element;

    constructor(element: Element) {
        this.element = element;
    }

    public attached(): void {
        if (!componentHandler) {
            throw 'componentHandler is not defined. Material library is required.';
        }

        let attribute = this.element.attributes['mt-mdl'];

        let typeName = attribute.value;

        let control = MdlControls[typeName];

        if (!control) {
            throw `${typeName} materialization not supported.`;
        }

        const _this = this;

        control.classes.forEach((item: string) => {
            _this.element.classList.add(item);
        });

        componentHandler.upgradeElement(this.element, control.type);

        if (!control.rippleEffectSupport) {
            return;
        }

        let addRippleEffect = this.element.classList.contains('mdl-js-ripple-effect');

        if (addRippleEffect) {
            componentHandler.upgradeElement(this.element, 'MaterialRipple');
        }

        let elements = this.element.getElementsByClassName('.mdl-js-ripple-effect') as any;

        for (let item of elements) {
            componentHandler.upgradeElement(item, 'MaterialRipple');
        }
    }
}