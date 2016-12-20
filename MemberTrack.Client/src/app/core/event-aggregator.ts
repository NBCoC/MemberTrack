import { Injectable, EventEmitter } from '@angular/core';

@Injectable()
export class EventAggregator {
    public loading: EventEmitter<boolean>;

    constructor() {
        this.loading = new EventEmitter<boolean>();
    }

    public isLoading(value: boolean): void {
        this.loading.emit(value);
    }
} 