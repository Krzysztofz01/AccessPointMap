import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from 'src/app/auth/services/auth.service';
import { Accesspoint } from 'src/app/models/accesspoint.model';
import { AccesspointDataService } from 'src/app/services/accesspoint-data.service';
import { AdminAccesspointMasterEditModalComponent } from '../admin-accesspoint-master-edit-modal/admin-accesspoint-master-edit-modal.component';

@Component({
  selector: 'admin-accesspoint-master',
  templateUrl: './admin-accesspoint-master.component.html',
  styleUrls: ['./admin-accesspoint-master.component.css']
})
export class AdminAccesspointMasterComponent implements OnInit {
  public accesspointContainer: Array<Accesspoint>;
  public searchQuery: string;
  public sortKey: string = 'id';
  public sortReverse: boolean = false;
  public page: number = 1;
  private token: string;

  constructor(private accesspointDataService: AccesspointDataService, private authService: AuthService, private modalService: NgbModal) { }

  ngOnInit(): void {
    this.token = this.authService.getToken();
    this.accesspointDataService.getAllAccessPointsAdmin(this.token)
      .subscribe((response) => {
        this.accesspointContainer = response;
      });
  }

  public deleteAccesspoint(accesspoint: Accesspoint): void {
    this.accesspointDataService.deleteAccesspoint(accesspoint.id, this.token)
      .subscribe((response) => {
        console.log(response);
      });
  }

  public editAccesspoint(accesspoint: Accesspoint): void {
    const ref = this.modalService.open(AdminAccesspointMasterEditModalComponent, { windowClass: 'custom-modal' });
    ref.componentInstance.accesspoint = accesspoint;

    ref.result.then((accesspoint) => {
      this.accesspointDataService.addOrUpdateAccesspoint(accesspoint, this.token)
        .subscribe((response) => {
          console.log(response);
        });
    },
    (exit) => {
      console.log('No changes');
    });
  }

  public displayAccesspoint(accesspoint: Accesspoint): void {
    this.accesspointDataService.displayAccesspoint(accesspoint.id, !accesspoint.display, this.token)
      .subscribe((response) => {
        console.log(response);
      });
  }

  public formatDates(accesspoint: Accesspoint): string {
    const cr = new Date(accesspoint.createDate);
    const up = new Date(accesspoint.updateDate);
    return `${cr.getDay()}.${cr.getMonth()}.${cr.getFullYear()}/${up.getDay()}.${up.getMonth()}.${up.getFullYear()}`;
  }

  public formatSecurity(accessPoint: Accesspoint): string {
    const secArray: Array<string> = JSON.parse(accessPoint.securityData);
    let outputFormat = '';
    secArray.forEach((element, index) => {
      if(index == 0) {
        outputFormat += element;
      } else {
        outputFormat += `/${element}`;
      }
    });
    return outputFormat;
  }

  public search(): void {
    this.page = 1;
    if(this.searchQuery == "") {
      this.ngOnInit();
    } else {
      this.accesspointContainer = this.accesspointContainer.filter(x => {
        return x.ssid.toLocaleLowerCase().match(this.searchQuery.toLocaleLowerCase()) ||
          x.bssid.toLocaleLowerCase().match(this.searchQuery.toLocaleLowerCase()) ||
          x.brand.toLocaleLowerCase().match(this.searchQuery.toLocaleLowerCase());
      });
    }
  }

  public sort(key: string): void {
    this.sortKey = key;
    this.sortReverse = !this.sortReverse;
  }
}
