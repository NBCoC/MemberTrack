declare var $: any;

export abstract class BaseDialogComponent {
    private dialog: any;

    constructor(private dialogId: string) { }

    protected initializeDialog(): void {
        this.dialog = $('#' + this.dialogId);
    }

    protected showDialog(): void {
        this.dialog.modal('show');
    }

    protected closeDialog(): void {
        this.dialog.modal('hide');
    }
}