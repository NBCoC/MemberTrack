import { customAttribute } from "aurelia-framework";

@customAttribute("mt-toggle-drawer")
export class ToggleDrawerAttribute {
    private element: Element;

    constructor(element: Element) {
        this.element = element;
    }

    public attached(): void {
        this.element.addEventListener("click", this.toggleDrawer);
    }

    public detached(): void {
        this.element.removeEventListener("click", this.toggleDrawer);
    }

    private toggleDrawer(e: Event): void {
        let layout = document.querySelector(".mdl-layout") as any;

        let shouldToggle = layout.classList.contains("is-small-screen");

        if (shouldToggle) {
            layout.MaterialLayout.toggleDrawer();
        }

        e.stopPropagation();
    }
}