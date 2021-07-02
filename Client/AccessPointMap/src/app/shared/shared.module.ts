import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from './navbar/navbar.component';
import { RouterModule } from '@angular/router';
import { MapComponent } from './map/map.component';
import { AccessPointDetailsComponent } from './access-point-details/access-point-details.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RichListComponent } from './rich-list/rich-list.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { NgxPaginationModule } from 'ngx-pagination';
import { OrderModule } from 'ngx-order-pipe';
import { AlertComponent } from './alert/alert.component';



@NgModule({
  declarations: [NavbarComponent, MapComponent, AccessPointDetailsComponent, RichListComponent, AlertComponent],
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    FormsModule,
    NgbModule,
    Ng2SearchPipeModule,
    NgxPaginationModule,
    OrderModule
  ],
  exports: [
    NavbarComponent,
    MapComponent,
    RichListComponent,
    AlertComponent,
    AccessPointDetailsComponent
  ]
})
export class SharedModule { }
