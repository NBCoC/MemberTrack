import { EventDispatcher } from '../core/event-dispatcher';
import { DatesDialogViewModel } from './dates-dialog.view-model';
import { PersonEvent } from '../core/custom-events';
import { PersonDto } from '../core/dtos';
import { bindable, customElement } from 'aurelia-framework';

@customElement('membertrack-dates')
export class DatesViewModel extends EventDispatcher {
    @bindable person: PersonDto = null;
    @bindable isEditor: boolean;
    public datesDialogVm: DatesDialogViewModel;

    constructor(element: Element) {
        super(element);
    }

    public displayDialog(): void {
        if (!this.isEditor) {
            return;
        }
        this.datesDialogVm.show(this.person.id, Object.assign({}, this.person.dates));
    }

    public dismissDialog(e: CustomEvent): boolean {
        let event = e.detail.args as PersonEvent;

        return this.dispatchEvent('change', event);
    }
}