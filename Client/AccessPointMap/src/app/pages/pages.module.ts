import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListModule } from './list/list.module';
import { LoginModule } from './login/login.module';
import { MainModule } from './main/main.module';
import { SearchModule } from './search/search.module';
import { StatisticsModule } from './statistics/statistics.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ListModule,
    LoginModule,
    MainModule,
    SearchModule,
    StatisticsModule
  ],
  exports: [
    ListModule,
    LoginModule,
    MainModule,
    SearchModule,
    StatisticsModule
  ]
})
export class PagesModule { }
