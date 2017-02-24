import { EventDispatcher } from './event-dispatcher';
import * as dialogPolyfill from 'dialog-polyfill/dialog-polyfill';

import { MdlHelper } from './mdl-helper';

export abstract class BaseDialog extends EventDispatcher {
    private dialogId: string;
    private dialog: any;

    constructor(element: Element, dialogId: string) {
        super(element);
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

    protected dismiss(args?: any): boolean {
        this.dialog.close();

        return this.dispatchEvent('dismiss', args);
    }

    public cancel(): void {
        this.dialog.close();
    }
}