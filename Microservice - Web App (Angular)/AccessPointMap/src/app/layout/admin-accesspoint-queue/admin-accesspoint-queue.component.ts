import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from 'src/app/auth/services/auth.service';
import { AccesspointQueue } from 'src/app/models/acesspoint-queue.model';
import { AccesspointDataService } from 'src/app/services/accesspoint-data.service';
import { AccesspointQueueDataService } from 'src/app/services/accesspoint-queue-data.service';
import { AccesspointViewModalComponent } from '../accesspoint-view-modal/accesspoint-view-modal.component';

@Component({
  selector: 'admin-accesspoint-queue',
  templateUrl: './admin-accesspoint-queue.component.html',
  styleUrls: ['./admin-accesspoint-queue.component.css']
})
export class AdminAccesspointQueueComponent implements OnInit {
  public accesspointContainer: Array<AccesspointQueue>;
  public searchQuery: string;
  public sortKey: string = 'id';
  public sortReverse: boolean = false;
  public page: number = 1;
  private token: string;

  constructor(private accesspointDataService: AccesspointDataService, private accesspointQueueDataService: AccesspointQueueDataService, private authService: AuthService, private modalService: NgbModal) { }

  ngOnInit(): void {
    this.token = this.authService.getToken();
    this.accesspointQueueDataService.getAllAccessPoints(this.token)
      .subscribe((response) => {
        this.accesspointContainer = response;
      });
  }

  public deleteAccesspoint(accesspoint: AccesspointQueue): void {
    this.accesspointQueueDataService.deleteAccesspoint(accesspoint.id, this.token)
      .subscribe((response) => {
        console.log(response);
      });
  }

  public viewAccesspoint(accesspoint: AccesspointQueue): void {
    const ref = this.modalService.open(AccesspointViewModalComponent, { size: 'lg' });
    ref.componentInstance.mapInit(accesspoint);
  }

  public mergeAccesspoint(accesspoint: AccesspointQueue): void {
    this.accesspointDataService.mergeAccesspoints([accesspoint.id], this.token)
      .subscribe((response) => {
        console.log(response);
      });
  }

  public formatDate(accesspoint: AccesspointQueue): string {
    const dt = new Date(accesspoint.createDate);
    return `${dt.getHours()}:${dt.getMinutes()} ${dt.getDay()}.${dt.getMonth()}.${dt.getFullYear()}`;
  }

  public search(): void {
    this.page = 1;
    if(this.searchQuery == "") {
      this.ngOnInit();
    } else {
      this.accesspointContainer = this.accesspointContainer.filter(x => {
        return x.ssid.toLocaleLowerCase().match(this.searchQuery.toLocaleLowerCase()) ||
          x.bssid.toLocaleLowerCase().match(this.searchQuery.toLocaleLowerCase());
      });
    }
  }

  public sort(key: string): void {
    this.sortKey = key;
    this.sortReverse = !this.sortReverse;
  }
}
