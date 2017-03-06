import { customElement } from 'aurelia-framework';

import { BaseDialog } from '../core/base-dialog';
import { UserService } from '../services/user.service';
import { LookupService } from '../services/lookup.service';
import { UserDto, LookupItemDto } from '../core/dtos';
import { UserEvent } from '../core/custom-events';

@customElement('mt-user-dialog')
export class UserDialogViewModel extends BaseDialog {
    private userService: UserService;
    private lookupService: LookupService;
    public model: UserDto;
    public roles: LookupItemDto[];

    constructor(element: Element, userService: UserService, lookupService: LookupService) {
        super(element, 'user-dialog');
        this.userService = userService;
        this.lookupService = lookupService;
        this.model = {} as UserDto;
        this.roles = [];
    }

    public attached(): void {
        this.register();

        this.lookupService.getAll().then(result => {
            if (!result) {
                return;
            }
            this.roles = result.roles;
        });
    }

    public show(model: UserDto): void {
        this.model = model;
        this.showModal();
    }

    public save(): void {
        if (!this.model.displayName || !this.model.email || !this.model.role) {
            return;
        }

        let func = this.model.id ?
            this.userService.update(this.model.id, this.model) :
            this.userService.insert(this.model);

        func.then(dto => {
            if (!dto) {
                return;
            }
            this.dismiss(new UserEvent(dto));
        });
    }
}