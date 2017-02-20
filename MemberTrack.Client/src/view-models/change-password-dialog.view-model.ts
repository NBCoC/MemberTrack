import { customElement } from 'aurelia-framework';
import { EventAggregator } from 'aurelia-event-aggregator';

import { BaseDialog } from '../core/base-dialog';
import { SnackbarEvent } from '../core/custom-events';
import { UserService } from '../services/user.service';
import { UpdatePasswordDto } from '../core/dtos';

@customElement('membertrack-change-password-dialog')
export class ChangePasswordDialogViewModel extends BaseDialog {
    private userService: UserService;
    private eventAggregator: EventAggregator;
    public model: UpdatePasswordDto;
    private contextUserId: number;

    constructor(element: Element, userService: UserService, eventAggregator: EventAggregator) {
        super(element, 'change-password-dialog');
        this.userService = userService;
        this.eventAggregator = eventAggregator;
        this.model = {} as UpdatePasswordDto;
    }

    public attached(argument: any): void {
        this.register();
    }

    public show(contextUserId: number): void {
        this.model = {} as UpdatePasswordDto;
        this.contextUserId = contextUserId;
        this.showModal();
    }

    public changePassword(e: Event): void {
        if (this.model.newPassword == null || this.model.oldPassword == null) {
            return;
        }

        this.userService.updatePassword(this.contextUserId, this.model).then(ok => {
            if (!ok) {
                return;
            }
            this.eventAggregator.publish(new SnackbarEvent('Password has been updated. Please sign in again...'));
            this.dismiss();
        });
    }
}