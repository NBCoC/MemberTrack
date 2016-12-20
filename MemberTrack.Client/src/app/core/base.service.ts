import { Headers, RequestOptions } from '@angular/http';

import { ToastService } from './toast.service';
import { TokenResponse } from './models';

const API_URL = 'http://localhost:5000/membertrack/';

export abstract class BaseService {
    private static tokenResponse: TokenResponse;
    private static toastService: ToastService;

    constructor(toastService: ToastService) {
        BaseService.toastService = toastService;
    }

    protected buildRequestOptions(model?: any): RequestOptions {
        let headers = new Headers();

        headers.append('Content-Type', 'application/json');

        if (BaseService.tokenResponse) {
            headers.append('Authorization', `${BaseService.tokenResponse.token_type} ${BaseService.tokenResponse.access_token}`);
        }

        return new RequestOptions({
            body: model ? JSON.stringify(model) : '',
            headers: headers
        });
    }

    protected getBaseUrl(): string {
        return API_URL + 'api/';
    }

    protected setTokenResponse(value: TokenResponse): void {
        BaseService.tokenResponse = value;
    }

    protected handleError(error: any): Promise<void> {
        if (error.status === 0) {
            BaseService.toastService.error('Failed to request data from API. Please contact System Administrator', null, error);
        } else {
            let data = JSON.parse(error._body);

            BaseService.toastService.error(data.message, null, error.status);
        }

        return Promise.reject(false);
    }
}