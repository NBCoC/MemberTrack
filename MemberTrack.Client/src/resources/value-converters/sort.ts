export class SortValueConverter {
    public toView(array: any[], propertyName: string, asc?: boolean): any[] {
        if (!array || array.length === 0) {
            return [];
        }

        if (!propertyName) {
            return array;
        }

        if (asc === undefined) {
            asc = true;
        }

        let func = (a: any, b: any) => {
            let result = 0;

            let condition = asc ? (a[propertyName] > b[propertyName]) : (b[propertyName] > a[propertyName]);

            if (condition) {
                result = 1;
            }

            condition = asc ? (a[propertyName] < b[propertyName]) : (b[propertyName] < a[propertyName]);

            if (condition) {
                result = -1;
            }

            return result;
        };

        return array.sort(func);
    }
}