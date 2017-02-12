import { autoinject, ObserverLocator } from 'aurelia-framework';

import { PersonService } from '../services/person.service';
import { PersonDto } from '../core/dtos';
import { BaseViewModel } from '../core/base-view-model';

@autoinject
export class SearchViewModel extends BaseViewModel {
    private personService: PersonService;
    private observerLocator: ObserverLocator;
    public people: PersonDto[];
    public hasResults: boolean;
    public searchText: string;
    private subscriber: any;

    constructor(personService: PersonService, observerLocator: ObserverLocator) {
        super('search');
        this.personService = personService;
        this.observerLocator = observerLocator;
        this.people = [];
        this.hasResults = true;
        this.searchText = '';
    }

    public attached(): void {
        const that = this;

        that.subscriber = that.observerLocator.getObserver(that, 'searchText').subscribe((newValue: string) => {
            that.personService.search(newValue).then(result => {
                if (!result) {
                    return;
                }
                that.people = result.data;
                that.hasResults = result.data.length > 0;
            });
        });
    }

    public detached(): void {
        if (!this.subscriber) {
            return;
        }
        this.subscriber.dispose();
    }
}