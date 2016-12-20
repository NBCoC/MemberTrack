import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

import { BaseService } from './base.service';
import { ToastService } from './toast.service';
import {
    Person
} from './models';

@Injectable()
export class PersonService extends BaseService {

    constructor(private http: Http, toastService: ToastService) {
        super(toastService);
    }

    public find(id: number): Promise<Person> {
        let url = this.getBaseUrl() + 'person/' + id;

        return this.http.get(url, this.buildRequestOptions())
            .toPromise()
            .then(response => response.json() as Person)
            .catch(this.handleError);
    }

    public remove(id: number): Promise<boolean> {
        let url = this.getBaseUrl() + 'person/' + id;

        return this.http.delete(url, this.buildRequestOptions())
            .toPromise()
            .then(response => {
                return true;
            })
            .catch(this.handleError);
    }

    public update(id: number, model: Person): Promise<Person> {
        let url = this.getBaseUrl() + 'person/' + id;

        let options = this.buildRequestOptions(model);

        return this.http.put(url, options.body, options)
            .toPromise()
            .then(response => response.json() as Person)
            .catch(this.handleError);
    }

    public insert(model: Person): Promise<Person> {
        let url = this.getBaseUrl() + 'person';

        let options = this.buildRequestOptions(model);

        return this.http.post(url, options.body, options)
            .toPromise()
            .then(response => response.json() as Person)
            .catch(this.handleError);
    }
}