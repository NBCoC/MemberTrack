import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'filterBy'
})
export class FilterByPipe implements PipeTransform {
    public transform(items: any[], propertyName: string, text: string): any[] {
        if (!items || items.length === 0) {
            return [];
        }

        if (!text) {
            return items;
        }

        text = text.toLocaleLowerCase();

        return items.filter(item => {
            let value = item[propertyName];

            if (!value) {
                return;
            }

            return value.toLocaleLowerCase().indexOf(text) !== -1;
        });
    }
}