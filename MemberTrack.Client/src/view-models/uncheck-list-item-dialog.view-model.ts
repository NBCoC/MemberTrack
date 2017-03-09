import { ValidationControllerFactory } from 'aurelia-validation';
import { customElement } from 'aurelia-framework';

import { BaseDialog } from '../core/base-dialog';
import { LookupItemDto } from '../core/dtos';
import { PromptEvent } from '../core/custom-events';

@customElement('mt-uncheck-list-item-dialog')
export class UncheckListItemDialogViewModel extends BaseDialog {
    public model: LookupItemDto = {} as LookupItemDto;

    constructor(element: Element, validationControllerFactory: ValidationControllerFactory) {
        super(validationControllerFactory, element, 'uncheck-list-item-dialog');
    }

    public attached(): void {
        this.register();
    }

    public show(name: string, id: number): void {
        this.model = { id: id, name: name } as LookupItemDto;

        this.showModal();
    }

    public ok(): void {
        this.dismiss(new PromptEvent(this.model.id));
    }

    protected registerValidation(): void { }
}