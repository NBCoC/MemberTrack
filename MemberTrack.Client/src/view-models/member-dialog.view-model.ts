import { customElement } from 'aurelia-framework';
import { ValidationControllerFactory, ValidationController, validateTrigger, ValidationRules } from 'aurelia-validation';
import { CustomValidationRenderer } from '../resources/validation/custom-validation-renderer';

import { BaseDialog } from '../core/base-dialog';
import { PersonService } from '../services/person.service';
import { LookupService } from '../services/lookup.service';
import { PersonDto, LookupItemDto } from '../core/dtos';
import { PersonEvent } from '../core/custom-events';

@customElement('mt-member-dialog')
export class MemberDialogViewModel extends BaseDialog {
    private personService: PersonService;
    private lookupService: LookupService;
    private validationController: ValidationController;
    public model: PersonDto;
    public genders: LookupItemDto[];
    public ageGroups: LookupItemDto[];
    public statusList: LookupItemDto[];

    constructor(element: Element, personService: PersonService, lookupService: LookupService,
        controllerFactory: ValidationControllerFactory) {
        super(element, 'member-dialog');
        this.personService = personService;
        this.lookupService = lookupService;
        this.validationController = controllerFactory.createForCurrentScope();

        this.validationController.addRenderer(new CustomValidationRenderer());
        this.validationController.validateTrigger = validateTrigger.change;

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
        this.registerValidaton();
        this.showModal();
    }

    public save(): void {
        this.validationController.validate().then(validationResult => {
            if (!validationResult.valid) {
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
        });
    }

    private registerValidaton(): void {
        ValidationRules
            .ensure('firstName').required()
            .ensure('lastName').required()
            .ensure('gender').required()
            .ensure('ageGroup').required()
            .ensure('status').required()
            .ensure('contactNumber').matches(/\b\d{3}[-]\d{3}[-]\d{4}\b/)
            .ensure('email').email()
            .on(this.model);

        this.validationController.validate();
    }
}