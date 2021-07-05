import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListModule } from './list/list.module';
import { LoginModule } from './login/login.module';
import { MainModule } from './main/main.module';
import { SearchModule } from './search/search.module';
import { StatisticsModule } from './statistics/statistics.module';
import { RouterModule } from '@angular/router';
import { PanelModule } from './panel/panel.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ListModule,
    LoginModule,
    MainModule,
    SearchModule,
    StatisticsModule,
    RouterModule,
    PanelModule
  ],
  exports: [
    ListModule,
    LoginModule,
    MainModule,
    SearchModule,
    StatisticsModule,
    PanelModule
  ]
})
export class PagesModule { }
