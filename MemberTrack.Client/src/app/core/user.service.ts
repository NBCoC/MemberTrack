import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

import { BaseService } from './base.service';
import { ToastService } from './toast.service';
import {
    ContextUser,
    User,
    SignIn,
    UpdatePassword,
    TokenResponse
} from './models';

@Injectable()
export class UserService extends BaseService {
    public contextUser: ContextUser;

    constructor(private http: Http, toastService: ToastService) {
        super(toastService);

        this.contextUser = {
            id: null,
            displayName: 'Welcome =)'
        } as ContextUser;
    }

    public signIn(model: SignIn): Promise<boolean> {
        let url = this.getBaseUrl() + 'token';

        let options = this.buildRequestOptions(model);

        return this.http.post(url, options.body, options)
            .toPromise()
            .then(response => response.json() as TokenResponse)
            .then(data => {
                this.setTokenResponse(data);

                url = this.getBaseUrl() + 'user/contextUser';

                return this.http.get(url, this.buildRequestOptions())
                    .toPromise()
                    .then(response => response.json() as ContextUser)
                    .then(user => {
                        this.contextUser = user;

                        return true;
                    });
            })
            .catch(this.handleError);
    }

    public getAll(): Promise<Array<User>> {
        let url = this.getBaseUrl() + 'user';

        return this.http.get(url, this.buildRequestOptions())
            .toPromise()
            .then(response => response.json() as Array<User>)
            .catch(this.handleError);
    }

    public remove(id: number): Promise<boolean> {
        let url = this.getBaseUrl() + 'user/' + id;

        return this.http.delete(url, this.buildRequestOptions())
            .toPromise()
            .then(response => {
                return true;
            })
            .catch(this.handleError);
    }

    public update(id: number, model: User): Promise<User> {
        let url = this.getBaseUrl() + 'user/' + id;

        let options = this.buildRequestOptions(model);

        return this.http.put(url, options.body, options)
            .toPromise()
            .then(response => response.json() as User)
            .catch(this.handleError);
    }

    public insert(model: User): Promise<User> {
        let url = this.getBaseUrl() + 'user';

        let options = this.buildRequestOptions(model);

        return this.http.post(url, options.body, options)
            .toPromise()
            .then(response => response.json() as User)
            .catch(this.handleError);
    }

    public updatePassword(id: number, model: UpdatePassword): Promise<boolean> {
        let url = this.getBaseUrl() + 'user/updatePassword/' + id;

        let options = this.buildRequestOptions(model);

        return this.http.put(url, options.body, options)
            .toPromise()
            .then(response => {
                return true;
            })
            .catch(this.handleError);
    }
}