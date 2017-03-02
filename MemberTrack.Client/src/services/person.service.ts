import { autoinject, LogManager } from 'aurelia-framework';
import { HttpClient } from 'aurelia-http-client';
import { EventAggregator } from 'aurelia-event-aggregator';

import { BaseService } from './base.service';
import {
    SearchResultDto, PersonDto, ChildrenInfoDto,
    PersonCheckListItemDto, DatesDto, PersonReportDto, RecentPersonDto
} from '../core/dtos';

@autoinject
export class PersonService extends BaseService {

    constructor(client: HttpClient, eventAggregator: EventAggregator) {
        super(client, LogManager.getLogger('PersonService'), eventAggregator);
    }

    public getRecentActivity(): Promise<RecentPersonDto[]> {
        return this.client.get('person/recentActivity')
            .then(result => JSON.parse(result.response))
            .catch(this.handleError);
    }

    public getReport(): Promise<PersonReportDto> {
        return this.client.get('person/report')
            .then(result => JSON.parse(result.response))
            .catch(this.handleError);
    }

    public search(contains: string): Promise<SearchResultDto> {
        return this.client.get(`person/search?contains=${contains}`)
            .then(result => JSON.parse(result.response))
            .catch(this.handleError);
    }

    public get(id: number): Promise<PersonDto> {
        return this.client.get(`person/${id}`)
            .then(result => JSON.parse(result.response))
            .catch(this.handleError);
    }

    public insert(dto: PersonDto): Promise<PersonDto> {
        return this.client.post('person', dto)
            .then(result => JSON.parse(result.response))
            .catch(this.handleError);
    }

    public update(id: number, dto: PersonDto): Promise<PersonDto> {
        return this.client.put(`person/${id}`, dto)
            .then(result => JSON.parse(result.response))
            .catch(this.handleError);
    }

    public remove(id: number): Promise<boolean> {
        return this.client.delete(`person/${id}`)
            .then(result => {
                return true;
            })
            .catch(this.handleError);
    }

    public insertOrUpdateChildrenInfo(id: number, dto: ChildrenInfoDto): Promise<PersonDto> {
        return this.client.post(`person/childrenInfo/${id}`, dto)
            .then(result => JSON.parse(result.response))
            .catch(this.handleError);
    }

    public updateDates(id: number, dto: DatesDto): Promise<PersonDto> {
        return this.client.post(`person/dates/${id}`, dto)
            .then(result => JSON.parse(result.response))
            .catch(this.handleError);
    }

    public insertOrRemoveCheckListItem(id: number, dto: PersonCheckListItemDto): Promise<PersonDto> {
        return this.client.post(`person/checkListItem/${id}`, dto)
            .then(result => JSON.parse(result.response))
            .catch(this.handleError);
    }
}