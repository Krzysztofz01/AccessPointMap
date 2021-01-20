import { Component, Input, OnInit } from '@angular/core';
import { AuthService } from 'src/app/auth/services/auth.service';
import { AccesspointDataService } from 'src/app/services/accesspoint-data.service';

@Component({
  selector: 'admin-accesspoint-add',
  templateUrl: './admin-accesspoint-add.component.html',
  styleUrls: ['./admin-accesspoint-add.component.css']
})
export class AdminAccesspointAddComponent implements OnInit {
  @Input() textData: string;
  @Input() forceMasterCheck: boolean;

  constructor(private accesspointDataService: AccesspointDataService, private authService: AuthService) { }

  ngOnInit(): void {
    this.textData = "";
    this.forceMasterCheck = false;
  }

  public upload(): void {
    const uploadData: Array<any> = JSON.parse(this.textData);

    const email = this.authService.getEmail();
    uploadData.forEach(x => {
      x.postedBy = email;
    });

    if(this.forceMasterCheck) {
      this.accesspointDataService.addOrUpdateAccesspointsMaster(uploadData, this.authService.getToken())
        .subscribe((response) => {
          console.log(response);
        });
    } else {
      this.accesspointDataService.addOrUpdateAccesspointsQueue(uploadData, this.authService.getToken())
        .subscribe((response) => {
          console.log(response);
        });
    }
  }

}
