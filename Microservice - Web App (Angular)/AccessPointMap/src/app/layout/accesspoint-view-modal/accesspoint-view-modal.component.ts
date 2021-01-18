import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-accesspoint-view-modal',
  templateUrl: './accesspoint-view-modal.component.html',
  styleUrls: ['./accesspoint-view-modal.component.css']
})
export class AccesspointViewModalComponent implements OnInit {

  constructor(public modal: NgbActiveModal) { }

  ngOnInit(): void {
  }

}
