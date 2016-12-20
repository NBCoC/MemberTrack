import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { UserService } from './user.service';
import { ToastService } from './toast.service';
import { UserRoles, RouteData } from './models';

@Injectable()
export class AuthGuard implements CanActivate {

    constructor(private router: Router, private userService: UserService, private toastService: ToastService) { }

    public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        let user = this.userService.contextUser;

        let data = route.data[0] as RouteData;

        let redirect = (message: string) => {
            this.toastService.info(message);

            this.router.navigate(['/sign-in']);

            return false;
        };

        if (!user.id || !data) {
            return redirect('Please sign in to use this application!');
        }

        let administrator = user.role === UserRoles.Admin || user.role === UserRoles.SystemAdmin;

        if (data.isAdmin && !administrator) {
            return redirect('You are not authorize to navigate there!');
        }

        return true;
    }
}
