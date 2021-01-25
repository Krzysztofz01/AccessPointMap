import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from 'src/app/auth/services/auth.service';
import { AccesspointDataService } from 'src/app/services/accesspoint-data.service';

@Component({
  selector: 'app-admin-accesspoint-add-modal',
  templateUrl: './admin-accesspoint-add-modal.component.html',
  styleUrls: ['./admin-accesspoint-add-modal.component.css']
})
export class AdminAccesspointAddModalComponent {
  public uploadedAccesspoints: number = 0;
  public affectedAccesspoints: number = 0;
  public message: string = "";

  constructor(private accesspointDataService: AccesspointDataService, private authService: AuthService, public modal: NgbActiveModal) { }

  public upload(accesspoints: Array<any>, force: boolean): void {
    const email = this.authService.getEmail();
    accesspoints.forEach(x => {
      x.postedBy = email;
    });

    if(force) {
      this.accesspointDataService.addOrUpdateAccesspointsMaster(accesspoints, this.authService.getToken())
        .subscribe((response) => {
          this.message = "Upload successful";
          this.uploadedAccesspoints = response.rowsPosted;
          this.affectedAccesspoints = response.rowsAffected;
        },
        (error) => {
          console.log(error);
          this.message = error.name;
        });
    } else {
      this.accesspointDataService.addOrUpdateAccesspointsQueue(accesspoints, this.authService.getToken())
        .subscribe((response) => {
          this.message = "Upload successful";
          this.uploadedAccesspoints = response.rowsPosted;
          this.affectedAccesspoints = response.rowsAffected;
        },
        (error) => {
          console.log(error);
          this.message = error.name;
        });
    }
  }
}
