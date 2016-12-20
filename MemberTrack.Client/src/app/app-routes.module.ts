import { Routes, RouterModule } from '@angular/router';

import { AuthGuard, SignInGuard } from './core';

import { SignInComponent } from './sign-in';
import { HomeComponent } from './home';
import { PersonDetailsComponent } from './person-details';
import { UsersComponent } from './users';

import { NotFoundComponent } from './404.component';

const AppRoutes: Routes = [
    {
        path: '',
        redirectTo: '/home',
        pathMatch: 'full'
    }, {
        path: 'sign-in',
        component: SignInComponent,
        canActivate: [SignInGuard],
        data: [{ title: 'Sign In', isAdmin: false }]
    }, {
        path: 'home',
        component: HomeComponent,
        canActivate: [AuthGuard],
        data: [{ title: 'Home', isAdmin: false }]
    }, {
        path: 'home/person-details/:id',
        component: PersonDetailsComponent,
        canActivate: [AuthGuard],
        data: [{ title: 'Person Details', isAdmin: false }]
    }, {
        path: 'administration/users',
        component: UsersComponent,
        canActivate: [AuthGuard],
        data: [{ title: 'Users', isAdmin: true }]
    }, {
        path: '**', component: NotFoundComponent,
        data: [{ title: '404', isAdmin: false }]
    }
];

export const AppRoutesModule = RouterModule.forRoot(AppRoutes);