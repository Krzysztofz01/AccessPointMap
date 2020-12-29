import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { GlobalMapComponent } from './layout/global-map/global-map.component';
import { HttpClientModule } from '@angular/common/http';
import { AccesspointViewComponent } from './layout/accesspoint-view/accesspoint-view.component';
import { AccesspointTableComponent } from './layout/accesspoint-table/accesspoint-table.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SecurityChartComponent } from './layout/security-chart/security-chart.component';
import { ChartsModule } from 'ng2-charts';
import { AreaChartComponent } from './layout/area-chart/area-chart.component';
import { FrequencyChartComponent } from './layout/frequency-chart/frequency-chart.component';
import { BrandChartComponent } from './layout/brand-chart/brand-chart.component';
import { PageMainComponent } from './layout/page-main/page-main.component';
import { PageAdminComponent } from './layout/page-admin/page-admin.component';
import { AccesspointTableV2Component } from './layout/accesspoint-table-v2/accesspoint-table-v2.component';
import { FormsModule } from '@angular/forms';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { OrderModule } from 'ngx-order-pipe';
import { NgxPaginationModule } from 'ngx-pagination';

@NgModule({
  declarations: [
    AppComponent,
    GlobalMapComponent,
    AccesspointViewComponent,
    AccesspointTableComponent,
    SecurityChartComponent,
    AreaChartComponent,
    FrequencyChartComponent,
    BrandChartComponent,
    PageMainComponent,
    PageAdminComponent,
    AccesspointTableV2Component
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ChartsModule,
    NgbModule,
    FormsModule,
    Ng2SearchPipeModule,
    OrderModule,
    NgxPaginationModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
