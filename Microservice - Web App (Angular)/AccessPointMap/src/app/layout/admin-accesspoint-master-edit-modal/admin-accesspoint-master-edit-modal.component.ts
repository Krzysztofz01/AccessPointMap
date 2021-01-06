import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Accesspoint } from 'src/app/models/accesspoint.model';

@Component({
  selector: 'admin-accesspoint-master-edit-modal',
  templateUrl: './admin-accesspoint-master-edit-modal.component.html',
  styleUrls: ['./admin-accesspoint-master-edit-modal.component.css']
})
export class AdminAccesspointMasterEditModalComponent implements OnInit {
  public accesspointForm: FormGroup;
  public accesspoint: Accesspoint;

  constructor(public modal: NgbActiveModal) { }

  ngOnInit(): void {
    this.accesspointForm = new FormGroup({
      ssid: new FormControl(this.accesspoint.ssid, [Validators.required]),
      frequency: new FormControl(this.accesspoint.frequency, [Validators.required]),
      highSignalLevel: new FormControl(this.accesspoint.highSignalLevel, [Validators.required]),
      highLongitude: new FormControl(this.accesspoint.highLongitude, [Validators.required]),
      highLatitude: new FormControl(this.accesspoint.highLatitude, [Validators.required]),
      lowSignalLevel: new FormControl(this.accesspoint.lowSignalLevel, [Validators.required]),
      lowLatitude: new FormControl(this.accesspoint.lowLatitude, [Validators.required]),
      lowLongitude: new FormControl(this.accesspoint.lowLongitude, [Validators.required]),
      postedby: new FormControl(this.accesspoint.postedBy, [Validators.required]),
      securityDataRaw: new FormControl(this.accesspoint.securityDataRaw, [Validators.required])
    });
  }

  public saveChanges(): void {
    if(this.accesspointForm.valid) {
      const values = this.accesspointForm.value;
      this.accesspoint.ssid = values.ssid;
      this.accesspoint.frequency = values.frequency;
      this.accesspoint.highSignalLevel = values.highSignalLevel;
      this.accesspoint.highLongitude = values.highLongitude;
      this.accesspoint.highLatitude = values.highLatitude;
      this.accesspoint.lowSignalLevel = values.lowSignalLevel;
      this.accesspoint.lowLongitude = values.lowLongitude;
      this.accesspoint.lowLatitude = values.lowLatitude;
      this.accesspoint.postedBy = values.postedby;
      this.accesspoint.securityDataRaw = values.securityDataRaw;

      this.modal.close(this.accesspoint);
    }
  }
}
