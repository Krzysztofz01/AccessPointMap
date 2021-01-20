import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from 'src/app/auth/services/auth.service';
import { AccesspointQueue } from 'src/app/models/acesspoint-queue.model';
import { AccesspointDataService } from 'src/app/services/accesspoint-data.service';
import { AccesspointQueueDataService } from 'src/app/services/accesspoint-queue-data.service';
import { DateFormatingService } from 'src/app/services/date-formating.service';
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

  constructor(private accesspointDataService: AccesspointDataService, private accesspointQueueDataService: AccesspointQueueDataService, private authService: AuthService, private modalService: NgbModal, private dateService: DateFormatingService) { }

  ngOnInit(): void {
    this.token = this.authService.getToken();
    this.accesspointQueueDataService.getAllAccessPoints(this.token)
      .subscribe((response) => {
        this.accesspointContainer = response;
      });
  }

  public deleteAccesspoint(accesspoint: AccesspointQueue): void {
    const index = this.accesspointContainer.indexOf(accesspoint);
    this.accesspointContainer = this.accesspointContainer.slice(0, index).concat(this.accesspointContainer.slice(index + 1, this.accesspointContainer.length));
    
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
    const index = this.accesspointContainer.indexOf(accesspoint);
    this.accesspointContainer = this.accesspointContainer.slice(0, index).concat(this.accesspointContainer.slice(index + 1, this.accesspointContainer.length));

    this.accesspointDataService.mergeAccesspoints([accesspoint.id], this.token)
      .subscribe((response) => {
        console.log(response);
      });
  }

  public formatDate(accesspoint: AccesspointQueue): string {
    return this.dateService.single(accesspoint.createDate);
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
