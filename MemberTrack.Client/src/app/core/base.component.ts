import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';

import { RouteData } from './models';

export abstract class BaseComponent {

    constructor(protected route: ActivatedRoute, private titleService: Title) { }

    setTitle(): void {
        let data = this.route.snapshot.data[0] as RouteData;

        if (!data) {
            return;
        }

        let title = data.title;

        this.titleService.setTitle('Member Track - ' + title);
    }
}