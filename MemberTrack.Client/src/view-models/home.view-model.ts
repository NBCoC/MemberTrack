import { autoinject } from 'aurelia-framework';
import { Router } from 'aurelia-router';

import { MemberDialogViewModel } from '../view-models/member-dialog.view-model';
import { PersonService } from '../services/person.service';
import { PersonEvent } from '../core/custom-events';

@autoinject
export class HomeViewModel {
    private personService: PersonService;
    private router: Router;
    public memberDialogVm: MemberDialogViewModel;

    constructor(personService: PersonService, router: Router) {
        this.personService = personService;
        this.router = router;
    }

    public displayMemberDialog(): void {
        this.memberDialogVm.show();
    }

    public dismissMemberDialog(e: CustomEvent): void {
        let event = e.detail.args as PersonEvent;
        this.router.navigateToRoute('member-details', { id: event.data.id });
    }
}