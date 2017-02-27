import { MdlHelper } from '../core/mdl-helper';
import { PersonService } from '../services/person.service';
import { EventDispatcher } from '../core/event-dispatcher';
import { PromptDeleteDialogViewModel } from './prompt-delete-dialog.view-model';
import { CheckListItemDialogViewModel } from './check-list-item-dialog.view-model';
import { DeleteItemEvent, PersonEvent } from '../core/custom-events';
import { DtoHelper, PersonCheckListItemDto, PersonDto } from '../core/dtos';
import { bindable, customElement } from 'aurelia-framework';

@customElement('membertrack-check-list')
export class CheckListViewModel extends EventDispatcher {
    @bindable person: PersonDto = null;
    public checkListItemDialogVm: CheckListItemDialogViewModel;
    public promptDeleteDialogVm: PromptDeleteDialogViewModel;
    private personService: PersonService;
    private dtoHelper: DtoHelper;

    constructor(element: Element, personService: PersonService, dtoHelper: DtoHelper) {
        super(element);
        this.personService = personService;
        this.dtoHelper = dtoHelper;
    }

    public dismissDialog(e: CustomEvent): boolean {
        let event = e.detail.args as PersonEvent;

        return this.dispatchEvent('change', event);
    }

    public checkListItemChange(e: Event, item: PersonCheckListItemDto): void {
        item.isSelected = !item.isSelected;

        this.initMdl();

        if (!item.date) {
            this.checkListItemDialogVm.show(this.person.id, Object.assign({}, item));
            return;
        }
        this.promptDeleteDialogVm.show(item.id, item.description, 'Uncheck');
    }

    public dismissPromptDeleteDialog(e: CustomEvent): void {
        let event = e.detail.args as DeleteItemEvent;

        let index = this.dtoHelper.getIndexOf(this.person.checkListItems, event.data);

        let model = this.person.checkListItems[index];

        this.personService.insertOrRemoveCheckListItem(this.person.id, model).then(dto => {
            if (!dto) {
                return;
            }

            return this.dispatchEvent('change', new PersonEvent(dto));
        });
    }

    private initMdl(): void {
        setTimeout(() => {
            MdlHelper.checkMdlComponents(this.element);
        });
    }
}