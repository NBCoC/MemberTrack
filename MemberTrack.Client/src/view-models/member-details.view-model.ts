import { PersonEvent, PromptEvent } from '../core/custom-events';
import { autoinject } from 'aurelia-framework';
import { BaseViewModel } from '../core/base-view-model';
import { PersonService } from '../services/person.service';
import { AuthService } from '../services/auth.service';
import { PersonDto } from '../core/dtos';
import { PromptDialogViewModel } from './prompt-dialog.view-model';
import { Router } from 'aurelia-router';

@autoinject
export class MemberDetailsViewModel extends BaseViewModel {
    private personService: PersonService;
    private authService: AuthService;
    private router: Router;
    public promptDialogVm: PromptDialogViewModel;
    public person: PersonDto;
    public isEditor: boolean;

    constructor(personService: PersonService, authService: AuthService, router: Router) {
        super('member-details');
        this.personService = personService;
        this.authService = authService;
        this.router = router;
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

    public displayPromptDialog(): void {
        if (!this.isEditor) {
            return;
        }
        let message = 'Are you sure want to delete this member / visitor? Note that this cannot be undone!';
        let name = this.person.firstName + ' ' + this.person.lastName;

        this.promptDialogVm.show('Delete', message, name, this.person.id);
    }

    public dismissPromptDialog(e: CustomEvent): void {
        let event = e.detail.args as PromptEvent;

        this.personService.remove(event.data).then(ok => {
            if (!ok) {
                return;
            }
            this.router.navigateToRoute('home');
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