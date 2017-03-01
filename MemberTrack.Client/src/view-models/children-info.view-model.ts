import { EventDispatcher } from '../core/event-dispatcher';
import { ChildrenInfoDialogViewModel } from './children-info-dialog.view-model';
import { PersonEvent } from '../core/custom-events';
import { PersonDto } from '../core/dtos';
import { bindable, customElement } from 'aurelia-framework';

@customElement('membertrack-children-info')
export class ChildrenInfoViewModel extends EventDispatcher {
    @bindable person: PersonDto = null;
    @bindable isEditor: boolean;
    public childrenInfoDialogVm: ChildrenInfoDialogViewModel;

    constructor(element: Element) {
        super(element);
    }

    public displayDialog(): void {
        this.childrenInfoDialogVm.show(this.person.id, Object.assign({}, this.person.childrenInfo));
    }

    public dismissDialog(e: CustomEvent): boolean {
        let event = e.detail.args as PersonEvent;

        return this.dispatchEvent('change', event);
    }
}