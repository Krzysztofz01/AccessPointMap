import { NgModule } from '@angular/core';
import { PanelComponent } from './panel.component';
import { QueueListComponent } from './queue-list/queue-list.component';
import { MergeModalComponent } from './merge-modal/merge-modal.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { NgxPaginationModule } from 'ngx-pagination';
import { OrderModule } from 'ngx-order-pipe';
import { CommonModule } from '@angular/common';
import { UsersListComponent } from './users-list/users-list.component';
import { UserStatComponent } from './user-stat/user-stat.component';
import { MobileAppComponent } from './mobile-app/mobile-app.component';
import { WorkshopComponent } from './workshop/workshop.component';
import { SettingsComponent } from './settings/settings.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [PanelComponent, QueueListComponent, MergeModalComponent, UsersListComponent, UserStatComponent, MobileAppComponent, WorkshopComponent, SettingsComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    NgbModule,
    Ng2SearchPipeModule,
    NgxPaginationModule,
    OrderModule,
    SharedModule
  ]
})
export class PanelModule { }
