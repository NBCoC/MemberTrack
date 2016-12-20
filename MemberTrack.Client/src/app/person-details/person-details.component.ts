import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';

import {
    BaseComponent,
    EventAggregator,
    PersonService,
    Person
} from '../core';

@Component({
    selector: 'member-track-person-details',
    templateUrl: './person-details.component.html'
})
export class PersonDetailsComponent extends BaseComponent implements OnInit {
    public model: Person;

    constructor(route: ActivatedRoute, titleService: Title, private eventAggregator: EventAggregator,
        private personService: PersonService) {
        super(route, titleService);
        this.model = {} as Person;
    }

    public ngOnInit(): void {
        this.setTitle();

        this.route.params
            .map(params => +params['id'])
            .subscribe((id: number) => {
                this.eventAggregator.isLoading(true);

                this.personService.find(id).then((data: Person) => {
                    this.eventAggregator.isLoading(false);

                    if (!data) {
                        return;
                    }

                    this.model = data;
                });
            });
    }
}