import { customElement } from "aurelia-framework";
import { EventAggregator } from "aurelia-event-aggregator";
import { BaseDialog } from "../core/base-dialog";
import { SnackbarEvent } from "../core/custom-events";
import { UserService } from "../services/user.service";
import { UpdatePasswordDto } from "../core/dtos";
import { ValidationControllerFactory, ValidationRules } from "aurelia-validation";

@customElement("mt-change-password-dialog")
export class ChangePasswordDialogViewModel extends BaseDialog {
    private userService: UserService;
    private eventAggregator: EventAggregator;
    public model: UpdatePasswordDto;
    private contextUserId: number;

    constructor(element: Element, userService: UserService, eventAggregator: EventAggregator,
        validationControllerFactory: ValidationControllerFactory) {
        super(validationControllerFactory, element, "change-password-dialog");
        this.userService = userService;
        this.eventAggregator = eventAggregator;
        this.model = {} as UpdatePasswordDto;
    }

    public attached(argument: any): void {
        this.register();
    }

    public show(contextUserId: number): void {
        this.model = {} as UpdatePasswordDto;
        this.contextUserId = contextUserId;
        this.showModal();
    }

    public changePassword(e: Event): void {
        this.validate().then(isValid => {
            if (!isValid) {
                return;
            }

            this.userService.updatePassword(this.contextUserId, this.model).then(ok => {
                if (!ok) {
                    return;
                }
                this.eventAggregator.publish(new SnackbarEvent("Password has been updated. Please sign in again..."));
                this.dismiss();
            });
        });
    }

    protected registerValidation(): void {
        ValidationRules
            .ensure("newPassword").required()
            .ensure("oldPassword").required()
            .on(this.model);
    }
}