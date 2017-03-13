import { ValidationControllerFactory } from "aurelia-validation";
import { customElement } from "aurelia-framework";

import { BaseDialog } from "../core/base-dialog";
import { PersonService } from "../services/person.service";
import { PersonCheckListItemDto } from "../core/dtos";
import { PersonEvent } from "../core/custom-events";

@customElement("mt-check-list-item-dialog")
export class CheckListItemDialogViewModel extends BaseDialog {
    private personService: PersonService;
    public model: PersonCheckListItemDto;
    public memberId: number;

    constructor(element: Element, personService: PersonService,
        validationControllerFactory: ValidationControllerFactory) {
        super(validationControllerFactory, element, "check-list-item-dialog");
        this.personService = personService;
        this.model = {} as PersonCheckListItemDto;
    }

    public attached(): void {
        this.register();
    }

    public show(memberId: number, model: PersonCheckListItemDto): void {
        this.memberId = memberId;
        this.model = model;
        this.showModal();
    }

    public save(): void {
        if (!this.memberId || !this.model.id) {
            return;
        }

        this.personService.insertOrRemoveCheckListItem(this.memberId, this.model).then(dto => {
            if (!dto) {
                return;
            }
            this.dismiss(new PersonEvent(dto));
        });
    }

    protected registerValidation(): void { }
}