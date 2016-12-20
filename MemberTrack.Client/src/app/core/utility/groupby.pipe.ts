import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'groupBy'
})
export class GroupByPipe implements PipeTransform {
    public transform(items: any[], propertyName: string): any[] {
        if (!items || items.length === 0) {
            return [];
        }

        if (!propertyName) {
            return items;
        }

        let groups = {};

        items.forEach(item => {
            let group = item[propertyName];

            groups[group] = groups[group] || [];

            groups[group].push(item);
        });

        return Object.keys(groups).map(group => {
            return {
                groupName: group,
                items: groups[group]
            };
        });
    }
}
