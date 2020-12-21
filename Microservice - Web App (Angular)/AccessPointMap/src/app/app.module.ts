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

@NgModule({
  declarations: [
    AppComponent,
    GlobalMapComponent,
    AccesspointViewComponent,
    AccesspointTableComponent,
    SecurityChartComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ChartsModule,
    NgbModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
