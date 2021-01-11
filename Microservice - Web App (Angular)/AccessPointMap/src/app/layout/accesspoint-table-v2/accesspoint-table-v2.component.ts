import { Component, OnInit } from '@angular/core';
import { Accesspoint } from 'src/app/models/accesspoint.model';
import { AccesspointDataService } from 'src/app/services/accesspoint-data.service';
import { CacheManagerService } from 'src/app/services/cache-manager.service';
import { ErrorHandlingService } from 'src/app/services/error-handling.service';
import { SelectedAccesspointService } from 'src/app/services/selected-accesspoint.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'accesspoint-table-v2',
  templateUrl: './accesspoint-table-v2.component.html',
  styleUrls: ['./accesspoint-table-v2.component.css']
})
export class AccesspointTableV2Component implements OnInit {
  public accesspointContainer: Array<Accesspoint>;
  public searchQuery: string;
  public sortKey: string = 'id';
  public sortReverse: boolean = false;
  public page: number = 1;

  constructor(private cacheService: CacheManagerService, private accesspointDataService: AccesspointDataService, private selectedAccesspoint: SelectedAccesspointService, private errorHandlingService: ErrorHandlingService) { }

  ngOnInit(): void {
    this.accesspointContainer = this.cacheService.load(environment.CACHE_ACCESSPOINTS);
    if(this.accesspointContainer == null) {
      this.accesspointDataService.getAllAccessPoints()
        .subscribe((response) => {
          this.accesspointContainer = response;
          this.cacheService.delete(environment.CACHE_ACCESSPOINTS);
          this.cacheService.save({ key: environment.CACHE_ACCESSPOINTS, data: response, expirationMinutes: 60 });
          this.accesspointContainer = this.accesspointContainer.map((accesspoint, i) => ({secName: this.getSecurityName(accesspoint), secColor: this.getSecurityColor(accesspoint), ...accesspoint}));
        },
        (error) => {
          console.log(error);
          this.errorHandlingService.setException(`${error.name} ${error.statusText}`);
        });
    }
    this.accesspointContainer = this.accesspointContainer.map((accesspoint, i) => ({secName: this.getSecurityName(accesspoint), secColor: this.getSecurityColor(accesspoint), ...accesspoint}));
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

  private changeSelectedAccesspoint(bssid: string): void {
    const accesspointByBssid = this.accesspointContainer.find(element => element.bssid == bssid);
    this.selectedAccesspoint.changeAccesspoint(accesspointByBssid);
  }

  private getSecurityColor(accesspoint: Accesspoint): string {
    const securityArray: Array<string> = JSON.parse(accesspoint.securityData);

    if(securityArray.includes('WPA2') || securityArray.includes('WPA')) return 'security-good';
    if(securityArray.includes('WPS') || securityArray.includes('WEP')) return 'security-average';
    return 'security-bad';
  }

  private getSecurityName(accesspoint: Accesspoint): string {
    const securityArray: Array<string> = JSON.parse(accesspoint.securityData);

    if(securityArray.includes('WPA2')) return 'WPA2';
    if(securityArray.includes('WPA')) return 'WPA';
    if(securityArray.includes('WPS')) return 'WPS';
    if(securityArray.includes('WEP')) return 'WEP';
    return 'None';
  }
}
