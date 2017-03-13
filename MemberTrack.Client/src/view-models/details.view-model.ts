import { EventDispatcher } from "../core/event-dispatcher";
import { MemberDialogViewModel } from "./member-dialog.view-model";
import { PersonEvent } from "../core/custom-events";
import { PersonDto } from "../core/dtos";
import { bindable, customElement } from "aurelia-framework";

@customElement("mt-details")
export class DetailsViewModel extends EventDispatcher {
    @bindable person: PersonDto = null;
    @bindable isEditor: boolean;
    public memberDialogVm: MemberDialogViewModel;

    constructor(element: Element) {
        super(element);
    }

    public displayDialog(): void {
        if (!this.isEditor) {
            return;
        }
        this.memberDialogVm.show(Object.assign({}, this.person));
    }

    public dismissDialog(e: CustomEvent): boolean {
        let event = e.detail.args as PersonEvent;

        return this.dispatchEvent("change", event);
    }
}