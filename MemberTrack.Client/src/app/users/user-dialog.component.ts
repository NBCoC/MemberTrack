import { Component, AfterViewInit, Output, EventEmitter } from '@angular/core';
import { NgForm } from '@angular/forms';

import {
    BaseDialogComponent,
    EventAggregator,
    UserService,
    LookupService,
    LookupResult,
    LookupItem,
    User
} from '../core';

@Component({
    selector: 'member-track-user-dialog',
    templateUrl: './user-dialog.component.html'
})
export class UserDialogComponent extends BaseDialogComponent implements AfterViewInit {
    public model: User;
    public roles: LookupItem[];

    @Output() public dismiss: EventEmitter<User>;

    constructor(private userService: UserService, private eventAggregator: EventAggregator,
        private lookupService: LookupService) {
        super('user-dialog');
        this.dismiss = new EventEmitter<User>();
        this.model = {} as User;
        this.roles = [];
    }

    public ngAfterViewInit(): void {
        this.initializeDialog();

        this.lookupService.getAll().then((data: LookupResult) => {
            if (!data) {
                return;
            }

            this.roles = data.roles;
        });
    }

    public show(model: User): void {
        this.model = model;
        this.showDialog();
    }

    public save(form: NgForm): void {
        if (!this.model.displayName || !this.model.email || !this.model.role) {
            return;
        }

        this.eventAggregator.isLoading(true);

        let func = this.model.id ?
            this.userService.update(this.model.id, this.model) :
            this.userService.insert(this.model);

        func.then((data: User) => {
            this.eventAggregator.isLoading(false);

            if (!data) {
                return;
            }

            form.reset();

            this.closeDialog();
            this.dismiss.emit(data);
        });
    }
}