import { ValidationControllerFactory, ValidationRules } from "aurelia-validation";
import { customElement } from "aurelia-framework";
import * as moment from "moment";

import { BaseDialog } from "../core/base-dialog";
import { PersonService } from "../services/person.service";
import { DatesDto } from "../core/dtos";
import { PersonEvent } from "../core/custom-events";

const Format = "MM/DD/YYYY";

@customElement("mt-dates-dialog")
export class DatesDialogViewModel extends BaseDialog {
    private personService: PersonService;
    public model: DatesDto;
    public memberId: number;

    constructor(element: Element, personService: PersonService, validationControllerFactory: ValidationControllerFactory) {
        super(validationControllerFactory, element, "dates-dialog");
        this.personService = personService;
        this.model = {} as DatesDto;
    }

    public attached(): void {
        this.register();
    }

    public show(memberId: number, model?: DatesDto): void {
        this.memberId = memberId;

        if (model) {
            if (model.baptismDate) {
                this.model.baptismDate = moment(model.baptismDate).format(Format) as any;
            } else {
                this.model.baptismDate = null;
            }

            if (model.firstVisitDate) {
                this.model.firstVisitDate = moment(model.firstVisitDate).format(Format) as any;
            } else {
                this.model.firstVisitDate = null;
            }

            if (model.membershipDate) {
                this.model.membershipDate = moment(model.membershipDate).format(Format) as any;
            } else {
                this.model.membershipDate = null;
            }
        }
        this.showModal();
    }

    public save(): void {
        if (!this.memberId) {
            return;
        }

        this.validate().then(isValid => {
            if (!isValid) {
                return;
            }

            let data = {
                baptismDate: !this.model.baptismDate ? null : new Date(this.model.baptismDate),
                membershipDate: !this.model.membershipDate ? null : new Date(this.model.membershipDate),
                firstVisitDate: !this.model.firstVisitDate ? null : new Date(this.model.firstVisitDate)
            } as DatesDto;

            this.personService.updateDates(this.memberId, data).then(dto => {
                if (!dto) {
                    return;
                }
                this.dismiss(new PersonEvent(dto));
            });
        });
    }

    protected registerValidation(): void {
        ValidationRules
            .ensure("baptismDate").matches(/\d+\/\d+\/\d{4}/)
            .ensure("membershipDate").matches(/\d+\/\d+\/\d{4}/)
            .ensure("firstVisitDate").matches(/\d+\/\d+\/\d{4}/)
            .on(this.model);
    }
}