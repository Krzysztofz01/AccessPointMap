import { Component, Input, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AdminAccesspointAddModalComponent } from '../admin-accesspoint-add-modal/admin-accesspoint-add-modal.component';

@Component({
  selector: 'admin-accesspoint-add',
  templateUrl: './admin-accesspoint-add.component.html',
  styleUrls: ['./admin-accesspoint-add.component.css']
})
export class AdminAccesspointAddComponent implements OnInit {
  @Input() textData: string;
  @Input() forceMasterCheck: boolean;

  constructor(private modalService: NgbModal) { }

  ngOnInit(): void {
    this.textData = "";
    this.forceMasterCheck = false;
  }

  public upload(): void {
    const uploadData: Array<any> = JSON.parse(this.textData);

    const ref = this.modalService.open(AdminAccesspointAddModalComponent);
    ref.componentInstance.upload(uploadData, this.forceMasterCheck);
  }
}
