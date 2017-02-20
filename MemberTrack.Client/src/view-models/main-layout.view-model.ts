import { ChangePasswordDialogViewModel } from './change-password-dialog.view-model';
import { autoinject, computedFrom } from 'aurelia-framework';
import { RouterConfiguration, Router } from 'aurelia-router';

import { AuthService } from '../services/auth.service';
import { UserDto } from '../core/dtos';

@autoinject
export class MainLayoutViewModel {
    public router: Router;
    private authService: AuthService;
    public contextUser: UserDto;
    public changePasswordDialogVm: ChangePasswordDialogViewModel;

    constructor(authService: AuthService) {
        this.authService = authService;
        this.contextUser = { displayName: 'Loading...' } as UserDto;
    }

    @computedFrom('router.currentInstruction')
    public get viewTitle(): string {
        return this.router.currentInstruction.config.title;
    }

    public configureRouter(config: RouterConfiguration, router: Router): void {
        config.map([
            {
                route: '',
                redirect: 'main/home'
            },
            {
                route: 'home',
                moduleId: 'view-models/home.view-model',
                name: 'home',
                title: 'Home',
                adminView: false,
                caseSensitive: true
            },
            {
                route: 'search',
                moduleId: 'view-models/search.view-model',
                name: 'search',
                title: 'Member Search',
                adminView: false,
                caseSensitive: true
            },
            {
                route: 'member-details/:id',
                moduleId: 'view-models/member-details.view-model',
                name: 'member-details',
                title: 'Member Details',
                adminView: false,
                caseSensitive: true
            },
            {
                route: 'administration/users',
                moduleId: 'view-models/users.view-model',
                name: 'users',
                title: 'User Administration',
                adminView: true,
                caseSensitive: true
            }
        ]);

        this.router = router;
    }

    public attached(argument: any): void {
        this.authService.getContextUser().then(user => {
            if (!user) {
                return;
            }
            this.contextUser = user;
        });
    }

    public signOut(e: Event): void {
        this.authService.signOut();
    }

    public displayChangePasswordDialog(): void {
        this.changePasswordDialogVm.show(this.contextUser.id);
    }

    public dismissChangePasswordDialog(e: Event): void {
        this.authService.signOut();
    }
}
