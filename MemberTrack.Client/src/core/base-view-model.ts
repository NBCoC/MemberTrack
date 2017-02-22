import { MdlHelper } from './mdl-helper';

export abstract class BaseViewModel {
    private viewId: string;
    public loadingText: string;

    constructor(viewId: string) {
        this.viewId = viewId;
        this.loadingText = 'Loading...';
    }

    protected initMdl(): void {
        let element = document.querySelector(`[membertrack-view="${this.viewId}"]`);

        setTimeout(() => {
            MdlHelper.checkMdlComponents(element);
        });
    }

    protected updateLoadingText(count: number): void {
        this.loadingText = count > 0 ? '' : 'No items available.';
    }
}