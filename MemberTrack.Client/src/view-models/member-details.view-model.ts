import { PersonEvent } from '../core/custom-events';
import { autoinject } from 'aurelia-framework';
import { BaseViewModel } from '../core/base-view-model';
import { PersonService } from '../services/person.service';
import { PersonDto } from '../core/dtos';

@autoinject
export class MemberDetailsViewModel extends BaseViewModel {
    private personService: PersonService;
    public person: PersonDto;

    constructor(personService: PersonService) {
        super('member-details');
        this.personService = personService;
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

    public personChanged(e: CustomEvent): void {
        if (!e.detail) {
            return;
        }

        let event = e.detail.args as PersonEvent;

        this.refreshModel(event.data);
    }

    private initDefaultModel(): void {
        this.person = {
            dates: {},
            address: {},
            childrenInfo: {},
            checkListItems: []
        } as PersonDto;
    }

    private refreshModel(dto: PersonDto): void {
        dto.checkListItems.forEach(item => {
            if (item.date) {
                item.isSelected = true;
            }
        });

        this.person = dto;
        this.initMdl();
    }
}