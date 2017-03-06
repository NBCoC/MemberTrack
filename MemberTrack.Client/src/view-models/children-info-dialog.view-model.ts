import { customElement } from 'aurelia-framework';

import { BaseDialog } from '../core/base-dialog';
import { PersonService } from '../services/person.service';
import { ChildrenInfoDto } from '../core/dtos';
import { PersonEvent } from '../core/custom-events';

@customElement('mt-children-info-dialog')
export class ChildrenInfoDialogViewModel extends BaseDialog {
    private personService: PersonService;
    public model: ChildrenInfoDto;
    public memberId: number;

    constructor(element: Element, personService: PersonService) {
        super(element, 'children-info-dialog');
        this.personService = personService;
        this.model = {} as ChildrenInfoDto;
    }

    public attached(): void {
        this.register();
    }

    public show(memberId: number, model?: ChildrenInfoDto): void {
        this.memberId = memberId;

        if (model) {
            this.model = model;
        }
        this.showModal();
    }

    public save(): void {
        if (!this.memberId) {
            return;
        }

        this.personService.insertOrUpdateChildrenInfo(this.memberId, this.model).then(dto => {
            if (!dto) {
                return;
            }
            this.dismiss(new PersonEvent(dto));
        });
    }
}