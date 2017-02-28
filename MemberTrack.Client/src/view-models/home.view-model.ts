import { autoinject } from 'aurelia-framework';
import { Router } from 'aurelia-router';

import { MemberDialogViewModel } from '../view-models/member-dialog.view-model';
import { PersonService } from '../services/person.service';
import { PersonEvent } from '../core/custom-events';
import { PersonReportDto } from '../core/dtos';

@autoinject
export class HomeViewModel {
    private personService: PersonService;
    private router: Router;
    public memberDialogVm: MemberDialogViewModel;
    public report: PersonReportDto;

    constructor(personService: PersonService, router: Router) {
        this.personService = personService;
        this.router = router;
    }

    public attached(): void {
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