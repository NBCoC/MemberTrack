import { Injectable } from '@angular/core';

import { ConsoleLoggerService } from './utility';

declare var toastr: any;

toastr.options = {
    'closeButton': false,
    'debug': false,
    'newestOnTop': false,
    'progressBar': true,
    'positionClass': 'toast-bottom-center',
    'preventDuplicates': true,
    'showDuration': '300',
    'hideDuration': '1000',
    'timeOut': '5000',
    'extendedTimeOut': '1000',
    'showEasing': 'swing',
    'hideEasing': 'linear',
    'showMethod': 'fadeIn',
    'hideMethod': 'fadeOut'
};

@Injectable()
export class ToastService {

    constructor(private logger: ConsoleLoggerService) { }

    public success(message: string, title?: string, data?: any): void {
        toastr.success(message, title);

        if (!data) {
            return;
        }

        this.logger.info(message, title, data);
    }

    public info(message: string, title?: string, data?: any): void {
        toastr.info(message, title);

        if (!data) {
            return;
        }

        this.logger.info(message, title, data);
    }

    public warning(message: string, title?: string, data?: any): void {
        toastr.warning(message, title);

        if (!data) {
            return;
        }

        this.logger.warn(message, title, data);
    }

    public error(message: string, title?: string, data?: any): void {
        toastr.error(message, title);

        if (!data) {
            return;
        }

        this.logger.error(message, title, data);
    }
}