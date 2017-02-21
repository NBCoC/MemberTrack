import { customElement } from 'aurelia-framework';
import * as moment from 'moment';

import { BaseDialog } from '../core/base-dialog';
import { PersonService } from '../services/person.service';
import { DatesDto } from '../core/dtos';
import { PersonEvent } from '../core/custom-events';

const Format = 'MM/DD/YYYY';

@customElement('membertrack-dates-dialog')
export class DatesDialogViewModel extends BaseDialog {
    private personService: PersonService;
    public model: DatesDto;
    public memberId: number;

    constructor(element: Element, personService: PersonService) {
        super(element, 'dates-dialog');
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
            }

            if (model.firstVisitDate) {
                this.model.firstVisitDate = moment(model.firstVisitDate).format(Format) as any;
            }

            if (model.membershipDate) {
                this.model.membershipDate = moment(model.membershipDate).format(Format) as any;
            }
        }
        this.showModal();
    }

    public save(): void {
        if (!this.memberId) {
            return;
        }

        let data = {
            baptismDate: new Date(this.model.baptismDate),
            membershipDate: new Date(this.model.membershipDate),
            firstVisitDate: new Date(this.model.firstVisitDate)
        } as DatesDto;

        this.personService.updateDates(this.memberId, data).then(dto => {
            if (!dto) {
                return;
            }
            this.dismiss(new PersonEvent(dto));
        });
    }
}