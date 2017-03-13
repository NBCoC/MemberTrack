import environment from "../environment";
import { HttpClient, RequestMessage, HttpResponseMessage } from "aurelia-http-client";
import { EventAggregator } from "aurelia-event-aggregator";

import { TokenDto } from "../core/dtos";
import { SnackbarEvent, IsLoadingEvent } from "../core/custom-events";

const TOKEN_CACHE_KEY = "membertrack.client.token.cache";

export abstract class BaseService {
    private static token: TokenDto = {} as TokenDto;
    private static logger: any;
    protected static eventAggregator: EventAggregator;

    protected client: HttpClient;

    constructor(client: HttpClient, logger: any, eventAggregator: EventAggregator) {
        BaseService.logger = logger;
        BaseService.eventAggregator = eventAggregator;

        const _that = this;

        client.configure(config => {
            config.withBaseUrl(environment.apiUrl);
            config.withHeader("Content-Type", "application/json");
            config.withInterceptor({
                request(message): RequestMessage {
                    BaseService.eventAggregator.publish(new IsLoadingEvent(true));

                    _that.loadTokenCache();

                    message.headers.add("Authorization", `${BaseService.token.token_type} ${BaseService.token.access_token}`);

                    return message;
                },
                response(message): HttpResponseMessage {
                    BaseService.eventAggregator.publish(new IsLoadingEvent(false));
                    return message;
                },
                requestError(error): RequestMessage {
                    BaseService.eventAggregator.publish(new IsLoadingEvent(false));
                    throw error;
                },
                responseError(error): HttpResponseMessage {
                    BaseService.eventAggregator.publish(new IsLoadingEvent(false));
                    throw error;
                }
            });
        });

        this.client = client;
    }

    protected setToken(value: TokenDto): void {
        BaseService.token = value;

        if (!this.isLocalStorageSupported()) {
            return;
        }

        localStorage.setItem(TOKEN_CACHE_KEY, JSON.stringify(value));
    }

    protected clearToken(): void {
        this.setToken({} as TokenDto);
    }

    private isLocalStorageSupported(): boolean {
        return (typeof (Storage) !== "undefined");
    }

    protected isTokenAvailable(): boolean {
        this.loadTokenCache();

        if (BaseService.token.access_token) {
            return true;
        }

        return false;
    }

    protected handleError(error: HttpResponseMessage): Promise<boolean> {
        let result = Promise.resolve(false);

        if (error.statusCode === 0) {
            let message = "Server error (Web API). Your token might have expired. Please refresh the browser. " +
                "If this error continues, please contact System Administrator.";

            BaseService.logger.error(message);
            BaseService.eventAggregator.publish(new SnackbarEvent(message));

            return result;
        }

        let data = JSON.parse(error.response);

        BaseService.logger.error(data.message);
        BaseService.eventAggregator.publish(new SnackbarEvent(data.message));

        return result;
    }

    protected loadTokenCache(): void {
        if (BaseService.token.access_token) {
            return;
        }

        if (this.isLocalStorageSupported()) {
            let tokenCache = localStorage.getItem(TOKEN_CACHE_KEY);

            if (tokenCache) {
                BaseService.token = JSON.parse(tokenCache) as TokenDto;
            }

            return;
        }

        BaseService.logger.warn("Local Storage is not available");
    }
}