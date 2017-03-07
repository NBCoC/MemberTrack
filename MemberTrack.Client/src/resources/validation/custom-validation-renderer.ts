import { RenderInstruction, ValidateResult } from 'aurelia-validation';

export class CustomValidationRenderer {

    public render(instruction: RenderInstruction) {
        for (let { result, elements } of instruction.unrender) {
            this.updateElement(result, elements, true);
        }

        for (let { result, elements } of instruction.render) {
            this.updateElement(result, elements, false);
        }
    }

    private updateElement(result: ValidateResult, elements: Element[], clear: boolean): void {
        elements.forEach(target => {
            let element = target.parentElement.querySelector('.mdl-textfield__error');

            element.textContent = clear ? '' : result.message;

            (element as any).style.visibility = clear ? 'hidden' : 'visible';
        });
    }
}