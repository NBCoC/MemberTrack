import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

import { BaseService } from './base.service';
import { ToastService } from './toast.service';
import { LookupResult } from './models';

@Injectable()
export class LookupService extends BaseService {
    private lookupResult: Promise<LookupResult>;

    constructor(private http: Http, toastService: ToastService) {
        super(toastService);
        this.lookupResult = null;
    }

    public getAll(): Promise<LookupResult> {
        if (!this.lookupResult) {
            let url = this.getBaseUrl() + 'lookup';

            this.lookupResult = this.http.get(url, this.buildRequestOptions())
                .toPromise()
                .then(response => response.json() as LookupResult)
                .catch(this.handleError);
        }

        return this.lookupResult;
    }
}