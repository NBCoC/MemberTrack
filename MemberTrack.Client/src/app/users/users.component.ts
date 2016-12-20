import { Component, OnInit, ViewChild } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';

import { PromptDeleteDialogComponent } from '../prompt-delete-dialog';
import { UserDialogComponent } from './user-dialog.component';

import {
    BaseComponent,
    UserService,
    EventAggregator,
    User,
    DeleteItem,
    ModelHelper
} from '../core';

@Component({
    selector: 'member-track-users',
    templateUrl: './users.component.html',
    styleUrls: ['./users.component.scss']
})
export class UsersComponent extends BaseComponent implements OnInit {
    public users: User[];
    @ViewChild(PromptDeleteDialogComponent) private promptDeleteDialog: PromptDeleteDialogComponent;
    @ViewChild(UserDialogComponent) private dialog: UserDialogComponent;

    constructor(route: ActivatedRoute, titleService: Title, private userService: UserService,
        private eventAggregator: EventAggregator, private modelHelper: ModelHelper) {
        super(route, titleService);
        this.users = [];
    }

    public ngOnInit(): void {
        this.setTitle();

        this.eventAggregator.isLoading(true);

        this.userService.getAll().then((data: User[]) => {
            this.eventAggregator.isLoading(false);

            if (!data) {
                return;
            }

            this.users = data;
        });
    }

    public displayDialog(e?: User): void {
        this.dialog.show(Object.assign({}, e));
    }

    public dismissDialog(e: User): void {
        this.modelHelper.insertOrUpdate(this.users, e);
    }

    public displayPromptDeleteDialog(e: User): void {
        this.promptDeleteDialog.prompt({ id: e.id, name: e.displayName });
    }

    public dismissPromptDeleteDialog(e: DeleteItem): void {
        this.eventAggregator.isLoading(true);

        this.userService.remove(e.id).then((data: boolean) => {
            this.eventAggregator.isLoading(false);

            if (!data) {
                return;
            }

            this.modelHelper.remove(this.users, e.id);
        });
    }
}