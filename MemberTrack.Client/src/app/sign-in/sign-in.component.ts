import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';

import { BaseComponent, UserService, EventAggregator, SignIn } from '../core';

@Component({
    selector: 'member-track-sign-in',
    templateUrl: './sign-in.component.html',
    styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent extends BaseComponent implements OnInit {

    constructor(route: ActivatedRoute, titleService: Title, private router: Router,
        private userService: UserService, private eventAggregator: EventAggregator) {
        super(route, titleService);
    }

    public ngOnInit(): void {
        this.setTitle();
    }

    public signIn(form: NgForm, email: string, password: string): void {
        if (!email || !password) {
            return;
        }

        this.eventAggregator.isLoading(true);

        let model = { email: email, password: password } as SignIn;

        this.userService.signIn(model).then((data: boolean) => {
            this.eventAggregator.isLoading(false);

            if (!data) {
                return;
            }

            form.reset();

            this.router.navigate(['/home']);
        });
    }
}