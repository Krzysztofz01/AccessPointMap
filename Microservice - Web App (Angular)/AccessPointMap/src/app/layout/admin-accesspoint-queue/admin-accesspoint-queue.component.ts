import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/auth/services/auth.service';
import { AccesspointQueue } from 'src/app/models/acesspoint-queue.model';
import { AccesspointDataService } from 'src/app/services/accesspoint-data.service';
import { AccesspointQueueDataService } from 'src/app/services/accesspoint-queue-data.service';

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

  constructor(private accesspointDataService: AccesspointDataService, private accesspointQueueDataService: AccesspointQueueDataService, private authService: AuthService) { }

  ngOnInit(): void {
    const token = this.authService.getToken();
    this.accesspointQueueDataService.getAllAccessPoints(token)
      .subscribe((response) => {
        this.accesspointContainer = response;
      });
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
