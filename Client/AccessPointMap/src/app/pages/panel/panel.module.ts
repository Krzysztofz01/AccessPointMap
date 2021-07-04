import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PanelComponent } from './panel.component';
import { QueueListComponent } from './queue-list/queue-list.component';
import { MergeModalComponent } from './merge-modal/merge-modal.component';



@NgModule({
  declarations: [PanelComponent, QueueListComponent, MergeModalComponent],
  imports: [
    CommonModule
  ]
})
export class PanelModule { }
