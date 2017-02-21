import { autoinject } from 'aurelia-framework';

import { BaseViewModel } from '../core/base-view-model';
import { PromptDeleteDialogViewModel } from '../view-models/prompt-delete-dialog.view-model';
import { MemberDialogViewModel } from './member-dialog.view-model';
import { AddressDialogViewModel } from './address-dialog.view-model';
import { ChildrenInfoDialogViewModel } from './children-info-dialog.view-model';
import { CheckListItemDialogViewModel } from './check-list-item-dialog.view-model';
import { DatesDialogViewModel } from './dates-dialog.view-model';
import { PersonService } from '../services/person.service';
import { LookupService } from '../services/lookup.service';
import { PersonDto, PersonCheckListItemDto, DtoHelper } from '../core/dtos';
import { PersonEvent, DeleteItemEvent } from '../core/custom-events';

@autoinject
export class MemberDetailsViewModel extends BaseViewModel {
    private personService: PersonService;
    private lookupService: LookupService;
    private dtoHelper: DtoHelper;
    public person: PersonDto;
    public promptDeleteDialogVm: PromptDeleteDialogViewModel;
    public memberDialogVm: MemberDialogViewModel;
    public addressDialogVm: AddressDialogViewModel;
    public childrenInfoDialogVm: ChildrenInfoDialogViewModel;
    public checkListItemDialogVm: CheckListItemDialogViewModel;
    public datesDialogVm: DatesDialogViewModel;

    constructor(personService: PersonService, lookupService: LookupService, dtoHelper: DtoHelper) {
        super('member-details');
        this.personService = personService;
        this.lookupService = lookupService;
        this.dtoHelper = dtoHelper;
        this.initDefaultModel();
    }

    public activate(params: any): void {
        let id = params.id as number;

        this.personService.get(id).then(dto => {
            if (!dto) {
                this.initDefaultModel();
                return;
            }
            this.refreshModel(dto);
        });
    }

    private initDefaultModel(): void {
        this.person = {
            dates: {},
            address: {},
            childrenInfo: {},
            checkListItems: []
        } as PersonDto;
    }

    public displayMemberDialog(): void {
        this.memberDialogVm.show(Object.assign({}, this.person));
    }

    public displayAddressDialog(): void {
        this.addressDialogVm.show(this.person.id, Object.assign({}, this.person.address));
    }

    public displayChildrenInfoDialog(): void {
        this.childrenInfoDialogVm.show(this.person.id, Object.assign({}, this.person.childrenInfo));
    }

    public displayDatesDialog(): void {
        this.datesDialogVm.show(this.person.id, Object.assign({}, this.person.dates));
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
            this.refreshModel(dto);
        });
    }

    public dismissDialog(e: CustomEvent): void {
        let event = e.detail.args as PersonEvent;

        this.refreshModel(event.data);
    }

    private refreshModel(dto: PersonDto): void {
        this.refreshCheckListItems(dto);
        this.person = dto;
        this.initMdl();
    }

    private refreshCheckListItems(dto: PersonDto): void {
        dto.checkListItems.forEach(item => {
            if (item.date) {
                item.isSelected = true;
            }
        });
    }
}