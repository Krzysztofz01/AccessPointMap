import { Component, OnInit } from '@angular/core';
import { AccessPoint } from 'src/app/core/models/access-point.model';
import { AccessPointService } from 'src/app/core/services/access-point.service';

@Component({
  selector: 'app-workshop',
  templateUrl: './workshop.component.html',
  styleUrls: ['./workshop.component.css']
})
export class WorkshopComponent implements OnInit {
  accessPointJsonData: string;

  constructor(private accessPointService: AccessPointService) { }

  ngOnInit(): void {
    this.accessPointJsonData = '';
  }

  //Read accesspoints data from clipboard and make a post request to server
  public pasteAndUpload(): void {
    if(this.accessPointJsonData.length) {
      const accessPoints: Array<AccessPoint> = JSON.parse(this.accessPointJsonData)

      this.accessPointService.postMany(accessPoints, 1).subscribe(() => {
        //TODO: Notification
      },
      (error) => {
        console.error(error);
      });
    }
  }
}
