import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/auth/services/auth.service';
import { Accesspoint } from 'src/app/models/accesspoint.model';
import { AccesspointDataService } from 'src/app/services/accesspoint-data.service';

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

  constructor(private accesspointDataService: AccesspointDataService, private authService: AuthService) { }

  ngOnInit(): void {
    const token = this.authService.getToken();
    this.accesspointDataService.getAllAccessPointsAdmin(token)
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
