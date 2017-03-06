import { customElement } from 'aurelia-framework';

import { BaseDialog } from '../core/base-dialog';
import { AddressService } from '../services/address.service';
import { LookupService } from '../services/lookup.service';
import { AddressDto, LookupItemDto } from '../core/dtos';
import { PersonEvent } from '../core/custom-events';

@customElement('mt-address-dialog')
export class AddressDialogViewModel extends BaseDialog {
    private addressService: AddressService;
    private lookupService: LookupService;
    public model: AddressDto;
    public memberId: number;
    public states: LookupItemDto[];

    constructor(element: Element, addressService: AddressService, lookupService: LookupService) {
        super(element, 'address-dialog');
        this.addressService = addressService;
        this.lookupService = lookupService;
        this.model = {} as AddressDto;
        this.states = [];
    }

    public attached(): void {
        this.register();

        this.lookupService.getAll().then(result => {
            if (!result) {
                return;
            }
            this.states = result.states;
        });
    }

    public show(memberId: number, model?: AddressDto): void {
        this.memberId = memberId;

        if (model) {
            this.model = model;
        }
        this.showModal();
    }

    public save(): void {
        if (!this.memberId) {
            return;
        }

        if (!this.model.city || !this.model.state ||
            !this.model.street || !this.model.zipCode) {
            return;
        }

        this.addressService.insertOrUpdate(this.memberId, this.model).then(dto => {
            if (!dto) {
                return;
            }
            this.dismiss(new PersonEvent(dto));
        });
    }
}