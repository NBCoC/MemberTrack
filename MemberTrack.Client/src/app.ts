import { autoinject } from 'aurelia-framework';
import { EventAggregator } from 'aurelia-event-aggregator';
import {
  Redirect,
  NavigationInstruction,
  RouterConfiguration,
  Next,
  Router
} from 'aurelia-router';

import { AuthService } from './services/auth.service';
import { IsLoadingEvent } from './core/custom-events';

@autoinject
export class App {
  private eventAggregator: EventAggregator;
  public router: Router;
  public isLoading: boolean;

  constructor(eventAggregator: EventAggregator) {
    this.eventAggregator = eventAggregator;
    this.isLoading = false;
  }

  public attached(argument: any): void {
    this.eventAggregator.subscribe(IsLoadingEvent, (e: IsLoadingEvent) => {
      if (this.isLoading === e.data) {
        return;
      }
      this.isLoading = e.data;
    });
  }

  public configureRouter(config: RouterConfiguration, router: Router): void {
    config.title = 'MemberTrack';
    config.addPipelineStep('authorize', AuthorizeStep);
    config.mapUnknownRoutes('view-models/404.view-model');
    config.fallbackRoute('main/home');
    config.map([
      {
        route: ['', 'main'],
        moduleId: 'view-models/main-layout.view-model',
        name: 'main',
        title: 'Main',
        caseSensitive: true
      },
      {
        route: 'sign-in',
        moduleId: 'view-models/sign-in.view-model',
        name: 'sign-in',
        title: 'Sign In',
        caseSensitive: true
      },
      {
        route: '401',
        moduleId: 'view-models/401.view-model',
        name: '401',
        title: '401',
        caseSensitive: true
      }
    ]);

    this.router = router;
  }
}

@autoinject
class AuthorizeStep {
  private authService: AuthService;

  constructor(authService: AuthService) {
    this.authService = authService;
  }

  public run(navigationInstruction: NavigationInstruction, next: Next): Promise<any> {
    let isSignInRoute = this.isSignInRoute(navigationInstruction.config.name);

    if (isSignInRoute && !this.authService.isSignedIn()) {
      return next();
    }

    if (isSignInRoute && this.authService.isSignedIn()) {
      return next.cancel();
    }

    return this.authService.isAuthenticated().then(isAuth => {
      if (isAuth) {
        return this.authService.getContextUser().then(user => {
          let isAdminView = navigationInstruction.getAllInstructions().some(i => (i.config as any).adminView);

          if (isAdminView && !user.isAdmin) {
            return next.cancel(new Redirect('401'));
          }

          return next();
        });
      }

      this.authService.redirectUrl = navigationInstruction.fragment;

      return next.cancel(new Redirect('sign-in'));
    });
  }

  private isSignInRoute(routeName: string): boolean {
    return routeName === 'sign-in';
  }
}