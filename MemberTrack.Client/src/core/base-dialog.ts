import * as dialogPolyfill from 'dialog-polyfill/dialog-polyfill';

import { MdlHelper } from './mdl-helper';

export abstract class BaseDialog {
    private dialogId: string;
    private dialog: any;
    private element: Element;

    constructor(element: Element, dialogId: string) {
        this.element = element;
        this.dialogId = dialogId;
    }

    protected register(): void {
        this.dialog = document.querySelector(`#${this.dialogId}`);

        if (this.dialog.showModal) {
            return;
        }

        dialogPolyfill.registerDialog(this.dialog);
    }

    protected showModal(): void {
        this.dialog.showModal();

        setTimeout(() => {
            MdlHelper.checkMdlComponents(this.element);
        });
    }

    protected dismiss(args?: any): void {
        let dismissEvent: CustomEvent;

        if ((window as any).CustomEvent) {
            dismissEvent = new CustomEvent('dismiss', {
                detail: {
                    args: args,
                }, bubbles: true
            });
        } else {
            dismissEvent = document.createEvent('CustomEvent');
            dismissEvent.initCustomEvent('dismiss', true, true, {
                detail: {
                    args: args
                }
            });
        }

        this.element.dispatchEvent(dismissEvent);
        this.dialog.close();
    }

    public cancel(): void {
        this.dialog.close();
    }
}