import { autoinject } from 'aurelia-framework';

import { PromptDialogViewModel } from '../view-models/prompt-dialog.view-model';
import { UserDialogViewModel } from '../view-models/user-dialog.view-model';
import { UserService } from '../services/user.service';
import { BaseViewModel } from '../core/base-view-model';
import { UserDto, DtoHelper } from '../core/dtos';
import { PromptEvent, UserEvent } from '../core/custom-events';

@autoinject
export class UsersViewModel extends BaseViewModel {
    private userService: UserService;
    private dtoHelper: DtoHelper;
    public users: UserDto[];
    public promptDialogVm: PromptDialogViewModel;
    public userDialogVm: UserDialogViewModel;

    constructor(userService: UserService, dtoHelper: DtoHelper) {
        super('users');
        this.userService = userService;
        this.dtoHelper = dtoHelper;
        this.users = [];
    }

    public attached(): void {
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
        this.promptDialogVm.show('Delete', 'Are you sure want to delete this user?', dto.displayName, dto.id);
    }

    public dismissPromptDialog(e: CustomEvent): void {
        let event = e.detail.args as PromptEvent;

        this.userService.remove(event.data).then(ok => {
            if (!ok) {
                return;
            }
            this.dtoHelper.remove(this.users, event.data);
            this.updateLoadingText(this.users.length);
        });
    }
}