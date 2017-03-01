import { autoinject } from 'aurelia-framework';
import { Router } from 'aurelia-router';

import { MemberDialogViewModel } from '../view-models/member-dialog.view-model';
import { PersonService } from '../services/person.service';
import { AuthService } from '../services/auth.service';
import { PersonEvent } from '../core/custom-events';
import { PersonReportDto } from '../core/dtos';

@autoinject
export class HomeViewModel {
    private personService: PersonService;
    private authService: AuthService;
    private router: Router;
    public memberDialogVm: MemberDialogViewModel;
    public report: PersonReportDto;
    public isEditor: boolean;

    constructor(personService: PersonService, router: Router, authService: AuthService) {
        this.personService = personService;
        this.authService = authService;
        this.router = router;
        this.isEditor = false;
    }

    public attached(): void {
        this.authService.getContextUser().then(user => {
            if (!user) {
                return;
            }
            this.isEditor = user.isEditor;
        });

        this.personService.getReport().then(data => {
            if (!data) {
                return;
            }
            this.report = data;
        });
    }

    public displayMemberDialog(): void {
        this.memberDialogVm.show();
    }

    public dismissMemberDialog(e: CustomEvent): void {
        let event = e.detail.args as PersonEvent;
        this.router.navigateToRoute('member-details', { id: event.data.id });
    }
}