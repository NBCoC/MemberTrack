import { customElement } from "aurelia-framework";
import { ValidationControllerFactory, ValidationRules } from "aurelia-validation";
import * as moment from "moment";

import { BaseDialog } from "../core/base-dialog";
import { PersonService } from "../services/person.service";
import { LookupService } from "../services/lookup.service";
import { PersonDto, LookupItemDto } from "../core/dtos";
import { PersonEvent } from "../core/custom-events";

const Format = "MM/DD/YYYY";

@customElement("mt-member-dialog")
export class MemberDialogViewModel extends BaseDialog {
    private personService: PersonService;
    private lookupService: LookupService;
    public model: PersonDto;
    public ageGroups: LookupItemDto[];
    public statusList: LookupItemDto[];

    constructor(element: Element, personService: PersonService, lookupService: LookupService,
        validationControllerFactory: ValidationControllerFactory) {
        super(validationControllerFactory, element, "member-dialog");
        this.personService = personService;
        this.lookupService = lookupService;

        this.model = {} as PersonDto;
        this.ageGroups = [];
        this.statusList = [];
    }

    public attached(): void {
        this.register();

        this.lookupService.getAll().then(result => {
            if (!result) {
                return;
            }
            this.ageGroups = result.ageGroups;
            this.statusList = result.personStatus;
        });
    }

    public show(model?: PersonDto): void {
        if (model) {
            this.model = model;

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
        } else {
            this.model.status = 0;
            this.model.ageGroup = 0;
        }
        this.showModal();
    }

    public save(): void {
        this.validate().then(isValid => {
            if (!isValid) {
                return;
            }

            let data = {} as PersonDto;

            Object.assign(data, this.model);

            data.membershipDate = !this.model.membershipDate ? null : new Date(this.model.membershipDate);
            data.firstVisitDate = !this.model.firstVisitDate ? null : new Date(this.model.firstVisitDate);

            let func = this.model.id ?
                this.personService.update(this.model.id, data) :
                this.personService.insert(data);

            func.then(dto => {
                if (!dto) {
                    return;
                }
                this.dismiss(new PersonEvent(dto));
            });
        });
    }

    protected registerValidation(): void {
        ValidationRules
            .ensure("fullName").required()
            .ensure("ageGroup").required()
            .ensure("status").required()
            .ensure("description").maxLength(500)
            .ensure("email").email()
            .ensure("contactNumber").matches(/\d{3}-\d{3}-\d{4}/)
            .ensure("membershipDate").matches(/\d+\/\d+\/\d{4}/)
            .ensure("firstVisitDate").matches(/\d+\/\d+\/\d{4}/)
            .on(this.model);
    }
}