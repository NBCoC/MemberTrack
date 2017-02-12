import { autoinject } from 'aurelia-framework';

import { PromptDeleteDialogViewModel } from '../view-models/prompt-delete-dialog.view-model';
import { UserDialogViewModel } from '../view-models/user-dialog.view-model';
import { UserService } from '../services/user.service';
import { BaseViewModel } from '../core/base-view-model';
import { UserDto, DtoHelper } from '../core/dtos';
import { DeleteItemEvent, UserEvent } from '../core/custom-events';

@autoinject
export class UsersViewModel extends BaseViewModel {
    private userService: UserService;
    private dtoHelper: DtoHelper;
    public users: UserDto[];
    public promptDeleteDialogVm: PromptDeleteDialogViewModel;
    public userDialogVm: UserDialogViewModel;

    constructor(userService: UserService, dtoHelper: DtoHelper) {
        super('users');
        this.userService = userService;
        this.dtoHelper = dtoHelper;
        this.users = [];
    }

    public attached(argument: any): void {
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

    public displayPromptDeleteDialog(dto: UserDto): void {
        this.promptDeleteDialogVm.show(dto.id, dto.displayName);
    }

    public dismissPromptDeleteDialog(e: CustomEvent): void {
        let event = e.detail.args as DeleteItemEvent;

        this.userService.remove(event.data).then(ok => {
            if (!ok) {
                return;
            }
            this.dtoHelper.remove(this.users, event.data);
            this.updateLoadingText(this.users.length);
        });
    }
}