import { autoinject, LogManager } from "aurelia-framework";
import { HttpClient } from "aurelia-http-client";
import { EventAggregator } from "aurelia-event-aggregator";

import { BaseService } from "./base.service";
import { LookupResultDto } from "../core/dtos";

@autoinject
export class LookupService extends BaseService {
    private lookupResultPromise: Promise<LookupResultDto>;

    constructor(client: HttpClient, eventAggregator: EventAggregator) {
        super(client, LogManager.getLogger("LookupService"), eventAggregator);
    }

    public getAll(): Promise<LookupResultDto> {
        if (!this.lookupResultPromise) {
            this.lookupResultPromise = this.client.get("lookup")
                .then(result => JSON.parse(result.response))
                .catch(this.handleError);
        }
        return this.lookupResultPromise;
    }
}