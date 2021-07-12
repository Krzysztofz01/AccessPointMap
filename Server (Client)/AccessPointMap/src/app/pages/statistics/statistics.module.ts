import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StatisticsComponent } from './statistics.component';
import { ChartsModule } from 'ng2-charts';



@NgModule({
  declarations: [StatisticsComponent],
  imports: [
    CommonModule,
    ChartsModule
  ]
})
export class StatisticsModule { }
