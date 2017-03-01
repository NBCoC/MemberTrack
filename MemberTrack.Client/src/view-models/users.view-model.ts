import { autoinject } from 'aurelia-framework';
import { EventAggregator } from 'aurelia-event-aggregator';

import { PromptDialogViewModel } from '../view-models/prompt-dialog.view-model';
import { UserDialogViewModel } from '../view-models/user-dialog.view-model';
import { UserService } from '../services/user.service';
import { AuthService } from '../services/auth.service';
import { BaseViewModel } from '../core/base-view-model';
import { UserDto, DtoHelper } from '../core/dtos';
import { PromptEvent, UserEvent, SnackbarEvent } from '../core/custom-events';

@autoinject
export class UsersViewModel extends BaseViewModel {
    private userService: UserService;
    private authService: AuthService;
    private eventAggregator: EventAggregator;
    private dtoHelper: DtoHelper;
    public users: UserDto[];
    public promptDialogVm: PromptDialogViewModel;
    public userDialogVm: UserDialogViewModel;
    private deleteMode: boolean;
    public isSystemAdmin: boolean;

    constructor(userService: UserService, authService: AuthService, eventAggregator: EventAggregator, dtoHelper: DtoHelper) {
        super('users');
        this.userService = userService;
        this.authService = authService;
        this.eventAggregator = eventAggregator;
        this.dtoHelper = dtoHelper;
        this.users = [];
        this.isSystemAdmin = false;
        this.deleteMode = true;
    }

    public attached(): void {
        this.authService.getContextUser().then(user => {
            if (!user) {
                return;
            }
            this.isSystemAdmin = user.isSystemAdmin;
        });

        this.userService.getAll().then(dtos => {
            if (!dtos) {
                return;
            }
            this.users = dtos;
            this.updateLoadingText(dtos.length);
        });
    }

    public displayUserDialog(dto?: UserDto): void {
        this.userDialogVm.show(Object.assign({}, dto));
    }

    public dismissUserDialog(e: CustomEvent): void {
        let event = e.detail.args as UserEvent;
        this.users = this.dtoHelper.insertOrUpdate(this.users, event.data);
        this.updateLoadingText(this.users.length);
    }

    public displayPromptDialog(dto: UserDto): void {
        this.deleteMode = true;
        this.promptDialogVm.show('Delete', 'Are you sure want to delete this user?', dto.displayName, dto.id);
    }

    public displayResetDialog(dto: UserDto): void {
        this.deleteMode = false;
        this.promptDialogVm.show('Reset Password for', 'Are you sure want to reset this user\'s password?', dto.displayName, dto.id);
    }

    public dismissPromptDialog(e: CustomEvent): void {
        let event = e.detail.args as PromptEvent;

        if (this.deleteMode) {
            this.userService.remove(event.data).then(ok => {
                if (!ok) {
                    return;
                }
                this.dtoHelper.remove(this.users, event.data);
                this.updateLoadingText(this.users.length);
            });

            return;
        }

        this.userService.resetPassword(event.data).then(ok => {
            if (!ok) {
                return;
            }
            this.eventAggregator.publish(new SnackbarEvent('Password has been reset!'));
        });
    }
}