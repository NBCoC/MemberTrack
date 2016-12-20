import {Injectable} from '@angular/core';

export interface ILogger {
    info: (message: string, caption?: string, data?: any) => void;
    error: (message: string, caption?: string, data?: any) => void;
    warn: (message: string, caption?: string, data?: any) => void;
}

@Injectable()
export class ConsoleLoggerService implements ILogger {

    public info(message: string, caption?: string, data?: any): void {
        console.info(caption + ' - ' + message, data || []);
    }

    public error(message: string, caption?: string, data?: any): void {
        console.error(caption + ' - ' + message, data || []);
    }

    public warn(message: string, caption?: string, data?: any): void {
        console.warn(caption + ' - ' + message, data || []);
    }
}