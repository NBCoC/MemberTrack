import { customElement } from 'aurelia-framework';

import { BaseDialog } from '../core/base-dialog';
import { LookupItemDto } from '../core/dtos';
import { PromptEvent } from '../core/custom-events';

@customElement('mt-prompt-dialog')
export class PromptDialogViewModel extends BaseDialog {
    public model: LookupItemDto = {} as LookupItemDto;
    public title: string;
    public message: string;

    constructor(element: Element) {
        super(element, 'prompt-dialog');
    }

    public attached(): void {
        this.register();
    }

    public show(title: string, message: string, name?: string, id?: number): void {
        this.title = title;
        this.message = message;
        this.model = { id: id, name: name } as LookupItemDto;

        this.showModal();
    }

    public ok(): void {
        this.dismiss(new PromptEvent(this.model.id));
    }
}