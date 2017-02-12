export class MdlHelper {

    public static checkMdlComponents(element: Element): void {
        if (!element) {
            return;
        }

        this.checkTextfield(element);
        this.checkCheckbox(element);
    }

    private static checkTextfield(element: Element): void {
        let inputs = element.querySelectorAll('.mdl-js-textfield');

        let count = inputs.length;

        for (let i = 0; i < count; i++) {
            let input = (inputs[i] as any);

            if (!input) {
                return;
            }

            input.MaterialTextfield.checkDirty();
            input.MaterialTextfield.checkValidity();
        }
    }

    private static checkCheckbox(element: Element): void {
        let inputs = element.querySelectorAll('.mdl-js-checkbox');

        let count = inputs.length;

        for (let i = 0; i < count; i++) {
            let input = (inputs[i] as any);

            if (!input) {
                return;
            }

            input.MaterialCheckbox.checkToggleState();
        }
    }
}