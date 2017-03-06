import { customElement } from 'aurelia-framework';

import { BaseDialog } from '../core/base-dialog';
import { PersonService } from '../services/person.service';
import { LookupService } from '../services/lookup.service';
import { PersonDto, LookupItemDto } from '../core/dtos';
import { PersonEvent } from '../core/custom-events';

@customElement('mt-member-dialog')
export class MemberDialogViewModel extends BaseDialog {
    private personService: PersonService;
    private lookupService: LookupService;
    public model: PersonDto;
    public genders: LookupItemDto[];
    public ageGroups: LookupItemDto[];
    public statusList: LookupItemDto[];

    constructor(element: Element, personService: PersonService, lookupService: LookupService) {
        super(element, 'member-dialog');
        this.personService = personService;
        this.lookupService = lookupService;
        this.model = {} as PersonDto;
        this.genders = [];
        this.ageGroups = [];
        this.statusList = [];
    }

    public attached(): void {
        this.register();

        this.lookupService.getAll().then(result => {
            if (!result) {
                return;
            }
            this.genders = result.genders;
            this.ageGroups = result.ageGroups;
            this.statusList = result.personStatus;
        });
    }

    public show(model?: PersonDto): void {
        if (model) {
            this.model = model;
        }
        this.showModal();
    }

    public save(): void {
        if (!this.model.firstName || !this.model.lastName ||
            !this.model.gender || !this.model.status) {
            return;
        }

        let func = this.model.id ?
            this.personService.update(this.model.id, this.model) :
            this.personService.insert(this.model);

        func.then(dto => {
            if (!dto) {
                return;
            }
            this.dismiss(new PersonEvent(dto));
        });
    }
}