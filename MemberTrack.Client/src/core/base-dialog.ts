import { EventDispatcher } from "./event-dispatcher";
import { ValidationControllerFactory, ValidationController, validateTrigger } from "aurelia-validation";
import { CustomValidationRenderer } from "../resources/validation/custom-validation-renderer";

import * as dialogPolyfill from "dialog-polyfill/dialog-polyfill";

import { MdlHelper } from "./mdl-helper";

export abstract class BaseDialog extends EventDispatcher {
    private dialogId: string;
    private dialog: any;
    private validationController: ValidationController;

    constructor(validationControllerFactory: ValidationControllerFactory, element: Element, dialogId: string) {
        super(element);
        this.validationController = validationControllerFactory.createForCurrentScope();

        this.validationController.addRenderer(new CustomValidationRenderer());
        this.validationController.validateTrigger = validateTrigger.change;

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

        this.registerValidation();

        this.validationController.validate();

        setTimeout(() => {
            MdlHelper.checkMdlComponents(this.element);
        });
    }

    protected abstract registerValidation(): void;

    protected validate(): Promise<boolean> {
        return this.validationController.validate().then(validationResult => {
            return validationResult.valid;
        });
    }

    protected dismiss(args?: any): boolean {
        this.dialog.close();

        return this.dispatchEvent("dismiss", args);
    }

    public cancel(): void {
        this.dialog.close();
    }
}