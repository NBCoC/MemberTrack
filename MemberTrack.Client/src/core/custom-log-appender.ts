import { Appender } from 'aurelia-logging';

export class CustomLogAppender implements Appender {

    public debug(logger: any, message: any, ...rest: any[]): void {
        console.debug(`DEBUG [${logger.id}] ${message}`, ...rest);
    }

    public info(logger: any, message: any, ...rest: any[]): void {
        console.info(`INFO [${logger.id}] ${message}`, ...rest);
    }

    public warn(logger: any, message: any, ...rest: any[]): void {
        console.warn(`WARN [${logger.id}] ${message}`, ...rest);
    }

    public error(logger: any, message: any, ...rest: any[]): void {
        console.error(`ERROR [${logger.id}] ${message}`, ...rest);
    }
}