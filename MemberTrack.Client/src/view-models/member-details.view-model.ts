import { PersonEvent } from '../core/custom-events';
import { autoinject } from 'aurelia-framework';
import { BaseViewModel } from '../core/base-view-model';
import { PersonService } from '../services/person.service';
import { AuthService } from '../services/auth.service';
import { PersonDto } from '../core/dtos';

@autoinject
export class MemberDetailsViewModel extends BaseViewModel {
    private personService: PersonService;
    private authService: AuthService;
    public person: PersonDto;
    public isEditor: boolean;

    constructor(personService: PersonService, authService: AuthService) {
        super('member-details');
        this.personService = personService;
        this.authService = authService;
        this.initDefaultModel();
        this.isEditor = false;
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

        this.authService.getContextUser().then(user => {
            if (!user) {
                return;
            }
            this.isEditor = user.isEditor;
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