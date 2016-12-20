import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { UserService } from './user.service';

@Injectable()
export class SignInGuard implements CanActivate {

    constructor(private router: Router, private userService: UserService) { }

    public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        if (this.userService.contextUser.id) {
            this.router.navigate(['/home']);
        }

        return true;
    }
}
