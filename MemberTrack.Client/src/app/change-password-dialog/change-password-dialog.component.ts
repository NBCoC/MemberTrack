import { Component, AfterViewInit } from '@angular/core';
import { NgForm } from '@angular/forms';

import {
    BaseDialogComponent,
    EventAggregator,
    UserService,
    UpdatePassword
} from '../core';

@Component({
    selector: 'member-track-change-password-dialog',
    templateUrl: './change-password-dialog.component.html'
})
export class ChangePasswordDialogComponent extends BaseDialogComponent implements AfterViewInit {

    constructor(private userService: UserService, private eventAggregator: EventAggregator) {
        super('change-password-dialog');
    }

    public ngAfterViewInit(): void {
        this.initializeDialog();
    }

    public show(): void {
        this.showDialog();
    }

    public save(form: NgForm, oldPassword: string, newPassword: string): void {
        if (!oldPassword || !newPassword) {
            return;
        }

        this.eventAggregator.isLoading(true);

        let id = this.userService.contextUser.id;

        let model = { oldPassword: oldPassword, newPassword: newPassword } as UpdatePassword;

        this.userService.updatePassword(id, model).then((data: boolean) => {
            this.eventAggregator.isLoading(false);

            if (!data) {
                return;
            }

            form.reset();

            this.closeDialog();
        });
    }
}