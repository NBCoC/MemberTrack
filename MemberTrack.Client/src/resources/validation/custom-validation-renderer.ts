import { RenderInstruction } from "aurelia-validation";

export class CustomValidationRenderer {

    public render(instruction: RenderInstruction) {
        for (let { elements } of instruction.unrender) {
            elements.forEach(target => {
                let element = target.parentElement.querySelector(".mdl-textfield__error");

                element.textContent = "";

                (element as any).style.visibility = "hidden";
            });
        }

        for (let { result, elements } of instruction.render) {
            elements.forEach(target => {
                let element = target.parentElement.querySelector(".mdl-textfield__error");

                if (result.valid) {
                    return;
                }

                element.textContent = result.message;

                (element as any).style.visibility = "visible";
            });
        }
    }
}