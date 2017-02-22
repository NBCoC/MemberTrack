import { autoinject, LogManager } from 'aurelia-framework';
import { HttpClient, HttpResponseMessage } from 'aurelia-http-client';
import { EventAggregator } from 'aurelia-event-aggregator';

import { BaseService } from './base.service';
import { SignInDto, TokenDto, UserDto } from '../core/dtos';

@autoinject
export class AuthService extends BaseService {
    private contextUserPromise: Promise<UserDto>;

    public redirectUrl: string = '';

    constructor(client: HttpClient, eventAggregator: EventAggregator) {
        super(client, LogManager.getLogger('AuthService'), eventAggregator);
    }

    public getContextUser(refresh?: boolean): Promise<UserDto> {
        if (!this.contextUserPromise || refresh) {
            this.contextUserPromise = this.client.get('user/contextUser')
                .then(result => JSON.parse(result.response))
                .catch(this.handleError);
        }
        return this.contextUserPromise;
    }

    public isAuthenticated(): Promise<boolean> {
        return this.client.get('user/isAuthenticated')
            .then(result => {
                return this.getContextUser().then(() => {
                    return true;
                });
            })
            .catch((error: HttpResponseMessage) => {
                this.clearToken();
                return false;
            });
    }

    public isSignedIn(): boolean {
        return this.isTokenAvailable();
    }

    public signIn(dto: SignInDto): Promise<boolean> {
        return this.client.post('token', dto)
            .then(result => JSON.parse(result.response))
            .then(token => {
                this.setToken(token as TokenDto);
                return true;
            }).catch(this.handleError);
    }

    public signOut(): void {
        this.clearToken();

        location.reload();
    }
}