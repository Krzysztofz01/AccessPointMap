import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from './navbar/navbar.component';
import { RouterModule } from '@angular/router';
import { MapComponent } from './map/map.component';
import { AccessPointDetailsComponent } from './access-point-details/access-point-details.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';



@NgModule({
  declarations: [NavbarComponent, MapComponent, AccessPointDetailsComponent],
  imports: [
    CommonModule,
    RouterModule,
    NgbModule
  ],
  exports: [
    NavbarComponent,
    MapComponent
  ]
})
export class SharedModule { }
