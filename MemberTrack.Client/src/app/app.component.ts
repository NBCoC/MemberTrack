import { Component, OnInit, ViewChild } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { ChangePasswordDialogComponent } from './change-password-dialog';

import {
  UserService,
  ContextUser,
  EventAggregator
} from './core';

@Component({
  selector: 'member-track',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  public isLoading: boolean;
  @ViewChild(ChangePasswordDialogComponent) private changePasswordDialog: ChangePasswordDialogComponent;

  constructor(private titleService: Title, private userService: UserService, private eventAggregator: EventAggregator) {
    this.isLoading = false;
  }

  public ngOnInit(): void {
    this.eventAggregator.loading.subscribe((value: boolean) => {
      this.isLoading = value;
    });
  }

  public get contextUser(): ContextUser {
    return this.userService.contextUser;
  }

  public get displayProgressBar(): boolean {
    return this.isLoading;
  }

  public get appTitle(): string {
    return this.titleService.getTitle() || 'Member Track';
  }

  public displayChangePasswordDialog(): void {
    this.changePasswordDialog.show();
  }
}
