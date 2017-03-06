import { EventDispatcher } from '../core/event-dispatcher';
import { PersonEvent } from '../core/custom-events';
import { PersonDto } from '../core/dtos';
import { AddressDialogViewModel } from './address-dialog.view-model';
import { bindable, customElement } from 'aurelia-framework';

@customElement('mt-address')
export class AddressViewModel extends EventDispatcher {
    @bindable person: PersonDto = null;
    @bindable isEditor: boolean;
    public addressDialogVm: AddressDialogViewModel;

    constructor(element: Element) {
        super(element);
    }

    public displayDialog(): void {
        if (!this.isEditor) {
            return;
        }
        this.addressDialogVm.show(this.person.id, Object.assign({}, this.person.address));
    }

    public dismissDialog(e: CustomEvent): boolean {
        let event = e.detail.args as PersonEvent;

        return this.dispatchEvent('change', event);
    }
}