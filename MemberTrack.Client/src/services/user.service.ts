import { autoinject, LogManager } from "aurelia-framework";
import { HttpClient } from "aurelia-http-client";
import { EventAggregator } from "aurelia-event-aggregator";

import { BaseService } from "./base.service";
import { UserDto, UpdatePasswordDto } from "../core/dtos";

@autoinject
export class UserService extends BaseService {

    constructor(client: HttpClient, eventAggregator: EventAggregator) {
        super(client, LogManager.getLogger("UserService"), eventAggregator);
    }

    public getAll(): Promise<UserDto[]> {
        return this.client.get("user")
            .then(result => JSON.parse(result.response))
            .catch(this.handleError);
    }

    public update(id: number, dto: UserDto): Promise<UserDto> {
        return this.client.put(`user/${id}`, dto)
            .then(result => JSON.parse(result.response))
            .catch(this.handleError);
    }

    public insert(dto: UserDto): Promise<UserDto> {
        return this.client.post("user", dto)
            .then(result => JSON.parse(result.response))
            .catch(this.handleError);
    }

    public updatePassword(id: number, dto: UpdatePasswordDto): Promise<boolean> {
        return this.client.put(`user/updatePassword/${id}`, dto)
            .then(result => {
                return true;
            })
            .catch(this.handleError);
    }

    public resetPassword(id: number): Promise<boolean> {
        return this.client.post(`user/resetPassword/${id}`, {})
            .then(result => {
                return true;
            })
            .catch(this.handleError);
    }

    public remove(id: number): Promise<boolean> {
        return this.client.delete(`user/${id}`)
            .then(result => {
                return true;
            })
            .catch(this.handleError);
    }
}