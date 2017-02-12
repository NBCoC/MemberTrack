import { customElement } from 'aurelia-framework';

import { BaseDialog } from '../core/base-dialog';
import { LookupItemDto } from '../core/dtos';
import { DeleteItemEvent } from '../core/custom-events';

@customElement('membertrack-prompt-delete-dialog')
export class PromptDeleteDialogViewModel extends BaseDialog {
    public model: LookupItemDto = {} as LookupItemDto;
    public title: string;

    constructor(element: Element) {
        super(element, 'prompt-delete-dialog');
        this.title = 'Delete';
    }

    public attached(): void {
        this.register();
    }

    public show(id: number, name: string, title?: string): void {
        if (title) {
            this.title = title;
        }

        this.model = { id: id, name: name } as LookupItemDto;
        this.showModal();
    }

    public ok(): void {
        this.dismiss(new DeleteItemEvent(this.model.id));
    }
}