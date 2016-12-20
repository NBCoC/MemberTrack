import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';

import { BaseComponent } from '../core';

@Component({
    selector: 'member-track-home',
    templateUrl: './home.component.html'
})
export class HomeComponent extends BaseComponent implements OnInit {

    constructor(route: ActivatedRoute, titleService: Title) {
        super(route, titleService);
    }

    public ngOnInit(): void {
        this.setTitle();
    }
}