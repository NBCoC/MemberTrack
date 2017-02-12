import * as moment from 'moment';

export class DateFormatValueConverter {
    public toView(date: Date, format: string): string {
        return moment(date).format(format);
    }
}