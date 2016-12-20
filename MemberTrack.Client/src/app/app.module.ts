import { NgModule } from '@angular/core';
import { BrowserModule, Title } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { LocationStrategy, HashLocationStrategy } from '@angular/common';

const RoutingStrategy = { provide: LocationStrategy, useClass: HashLocationStrategy };

import { AppRoutesModule } from './app-routes.module';
import { UtilityModule } from './core/utility';

import { AppComponent } from './app.component';
import { HomeComponent } from './home';
import { PersonDetailsComponent } from './person-details';
import { SignInComponent } from './sign-in';
import { UsersComponent, UserDialogComponent } from './users';
import { NotFoundComponent } from './404.component';
import { PromptDeleteDialogComponent } from './prompt-delete-dialog';
import { ChangePasswordDialogComponent } from './change-password-dialog';

import {
  AuthGuard,
  SignInGuard,
  ModelHelper,
  ToastService,
  UserService,
  LookupService,
  PersonService,
  EventAggregator
} from './core';

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpModule,
    AppRoutesModule,
    UtilityModule
  ],
  declarations: [
    AppComponent,
    HomeComponent,
    PersonDetailsComponent,
    SignInComponent,
    UsersComponent,
    UserDialogComponent,
    NotFoundComponent,
    PromptDeleteDialogComponent,
    ChangePasswordDialogComponent
  ],
  providers: [
    Title,
    ModelHelper,
    EventAggregator,
    LookupService,
    ToastService,
    UserService,
    PersonService,
    AuthGuard,
    SignInGuard,
    RoutingStrategy
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
