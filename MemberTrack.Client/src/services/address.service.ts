import { autoinject, LogManager } from "aurelia-framework";
import { HttpClient } from "aurelia-http-client";
import { EventAggregator } from "aurelia-event-aggregator";

import { BaseService } from "./base.service";
import { AddressDto, PersonDto } from "../core/dtos";

@autoinject
export class AddressService extends BaseService {

    constructor(client: HttpClient, eventAggregator: EventAggregator) {
        super(client, LogManager.getLogger("AddressService"), eventAggregator);
    }

    public insertOrUpdate(id: number, dto: AddressDto): Promise<PersonDto> {
        return this.client.post(`address/${id}`, dto)
            .then(result => JSON.parse(result.response))
            .catch(this.handleError);
    }

    public remove(id: number): Promise<boolean> {
        return this.client.delete(`address/${id}`)
            .then(result => {
                return true;
            })
            .catch(this.handleError);
    }
}