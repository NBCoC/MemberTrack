import { NgModule } from '@angular/core';

import { ConsoleLoggerService } from './console-logger.service';
import { OrderByPipe } from './orderby.pipe';
import { GroupByPipe } from './groupby.pipe';
import { FilterByPipe } from './filterby.pipe';

@NgModule({
    exports: [
        OrderByPipe,
        GroupByPipe,
        FilterByPipe
    ],
    declarations: [
        OrderByPipe,
        GroupByPipe,
        FilterByPipe
    ],
    providers: [
        ConsoleLoggerService
    ]
})
export class UtilityModule { }