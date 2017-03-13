import { customElement } from "aurelia-framework";
import { EventAggregator } from "aurelia-event-aggregator";

import { SnackbarEvent } from "../../core/custom-events";

@customElement("mt-snackbar")
export class Snackbar {
    private element: Element;
    private snackbarContainer: any;
    private eventAggregator: EventAggregator;
    private subscriber: any;

    constructor(element: Element, eventAggregator: EventAggregator) {
        this.element = element;
        this.eventAggregator = eventAggregator;
    }

    public attached(): void {
        this.snackbarContainer = this.element.querySelector(`div[mt-mdl="snackbar"]`) as any;

        this.subscriber = this.eventAggregator.subscribe(SnackbarEvent, (e: SnackbarEvent) => {
            let data = { message: e.data, timeout: 5000 };

            this.snackbarContainer.MaterialSnackbar.showSnackbar(data);
        });
    }

    public detached(): void {
        if (!this.subscriber) {
            return;
        }
        this.subscriber.dispose();
    }
}