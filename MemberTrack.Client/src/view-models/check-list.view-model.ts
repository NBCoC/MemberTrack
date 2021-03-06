import { MdlHelper } from "../core/mdl-helper";
import { PersonService } from "../services/person.service";
import { EventDispatcher } from "../core/event-dispatcher";
import { CheckListItemDialogViewModel } from "./check-list-item-dialog.view-model";
import { PromptEvent, PersonEvent } from "../core/custom-events";
import { DtoHelper, PersonCheckListItemDto, PersonDto } from "../core/dtos";
import { bindable, customElement } from "aurelia-framework";
import { UncheckListItemDialogViewModel } from "./uncheck-list-item-dialog.view-model";

@customElement("mt-check-list")
export class CheckListViewModel extends EventDispatcher {
    @bindable person: PersonDto = null;
    @bindable isEditor: boolean;
    public checkListItemDialogVm: CheckListItemDialogViewModel;
    public uncheckListItemDialogVm: UncheckListItemDialogViewModel;
    private personService: PersonService;
    private dtoHelper: DtoHelper;

    constructor(element: Element, personService: PersonService, dtoHelper: DtoHelper) {
        super(element);
        this.personService = personService;
        this.dtoHelper = dtoHelper;
    }

    public dismissDialog(e: CustomEvent): boolean {
        let event = e.detail.args as PersonEvent;

        return this.dispatchEvent("change", event);
    }

    public checkListItemChange(e: Event, item: PersonCheckListItemDto): void {
        item.isSelected = !item.isSelected;

        this.initMdl();

        if (!this.isEditor) {
            return;
        }

        if (!item.date) {
            this.checkListItemDialogVm.show(this.person.id, Object.assign({}, item));
            return;
        }
        this.uncheckListItemDialogVm.show(item.description, item.id);
    }

    public dismissUncheckListItemDialog(e: CustomEvent): void {
        let event = e.detail.args as PromptEvent;

        let index = this.dtoHelper.getIndexOf(this.person.checkListItems, event.data);

        let model = this.person.checkListItems[index];

        this.personService.insertOrRemoveCheckListItem(this.person.id, model).then(dto => {
            if (!dto) {
                return;
            }

            return this.dispatchEvent("change", new PersonEvent(dto));
        });
    }

    private initMdl(): void {
        setTimeout(() => {
            MdlHelper.checkMdlComponents(this.element);
        });
    }
}