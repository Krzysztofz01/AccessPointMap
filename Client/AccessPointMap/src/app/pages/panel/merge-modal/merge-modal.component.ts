import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccessPoint } from 'src/app/core/models/access-point.model';

@Component({
  selector: 'app-merge-modal',
  templateUrl: './merge-modal.component.html',
  styleUrls: ['./merge-modal.component.css']
})
export class MergeModalComponent implements OnInit {
  @Output() changeEvent = new EventEmitter<AccessPoint>();
  @Output() deleteEvent = new EventEmitter<AccessPoint>();

  private masterAccessPoint: AccessPoint;
  public queueAccessPoint: AccessPoint;

  constructor() { }

  ngOnInit(): void {
  }

}
