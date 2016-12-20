import { Component, AfterViewInit, Output, EventEmitter } from '@angular/core';

import { BaseDialogComponent, DeleteItem } from '../core';

@Component({
    selector: 'member-track-prompt-delete-dialog',
    templateUrl: './prompt-delete-dialog.component.html',
    styleUrls: ['./prompt-delete-dialog.component.scss']
})
export class PromptDeleteDialogComponent extends BaseDialogComponent implements AfterViewInit {
    public model: DeleteItem;

    @Output() public dismiss: EventEmitter<DeleteItem>;

    constructor() {
        super('prompt-delete-dialog');
        this.dismiss = new EventEmitter<DeleteItem>();
        this.model = {} as DeleteItem;
    }

    public ngAfterViewInit(): void {
        this.initializeDialog();
    }

    public prompt(model: DeleteItem): void {
        this.model = model;
        this.showDialog();
    }

    public dismissDialog(): void {
        this.closeDialog();
        this.dismiss.emit(this.model);
    }
}