import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'orderBy'
})
export class OrderByPipe implements PipeTransform {
    public transform(items: any[], propertyName: string, asc?: boolean): any[] {
        if (!items || items.length === 0) {
            return [];
        }

        if (!propertyName) {
            return items;
        }

        if (asc === undefined) {
            asc = true;
        }

        let func = (a: any, b: any) => {
            let confition = (asc ? a[propertyName] > b[propertyName] : b[propertyName] > a[propertyName]);

            if (confition) {
                return 1;
            }

            confition = (asc ? a[propertyName] < b[propertyName] : b[propertyName] < a[propertyName]);

            if (confition) {
                return -1;
            }

            return 0;
        };

        return items.sort(func);
    }
}