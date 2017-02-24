export abstract class EventDispatcher {
    protected element: Element;

    constructor(element: Element) {
        this.element = element;
    }

    protected dispatchEvent(eventName: string, args?: any): boolean {
        let customEvent: CustomEvent;

        if ((window as any).CustomEvent) {
            customEvent = new CustomEvent(eventName, {
                detail: {
                    args: args,
                }, bubbles: true
            });
        } else {
            customEvent = document.createEvent('CustomEvent');

            customEvent.initCustomEvent(eventName, true, true, {
                detail: {
                    args: args
                }
            });
        }

        this.element.dispatchEvent(customEvent);

        return true;
    }
}