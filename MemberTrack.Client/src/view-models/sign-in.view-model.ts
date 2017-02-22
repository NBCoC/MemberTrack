import { autoinject } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { EventAggregator } from 'aurelia-event-aggregator';

import { AuthService } from '../services/auth.service';
import { SignInDto } from '../core/dtos';
import { SnackbarEvent } from '../core/custom-events';

@autoinject
export class SignInViewModel {
    private authService: AuthService;
    private router: Router;
    private eventAggregator: EventAggregator;

    public model: SignInDto = {} as SignInDto;

    constructor(router: Router, authService: AuthService, eventAggregator: EventAggregator) {
        this.router = router;
        this.authService = authService;
        this.eventAggregator = eventAggregator;
    }

    public signIn(e: Event): void {
        this.authService.signIn(this.model).then(ok => {
            if (!ok) {
                return;
            }
            this.eventAggregator.publish(new SnackbarEvent('Welcome =)'));

            this.router.navigate(this.authService.redirectUrl || 'main/home');
        });
    }
}