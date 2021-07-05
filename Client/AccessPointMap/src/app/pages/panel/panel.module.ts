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

@NgModule({
  declarations: [PanelComponent, QueueListComponent, MergeModalComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    NgbModule,
    Ng2SearchPipeModule,
    NgxPaginationModule,
    OrderModule
  ]
})
export class PanelModule { }
