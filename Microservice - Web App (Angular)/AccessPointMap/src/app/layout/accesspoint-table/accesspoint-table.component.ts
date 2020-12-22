import { Component, OnInit } from '@angular/core';
import { Accesspoint } from 'src/app/models/accesspoint.model';
import { CacheManagerService } from 'src/app/services/cache-manager.service';
import { SelectedAccesspointService } from 'src/app/services/selected-accesspoint.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'accesspoint-table',
  templateUrl: './accesspoint-table.component.html',
  styleUrls: ['./accesspoint-table.component.css']
})
export class AccesspointTableComponent implements OnInit {
  private accesspointContainer: Array<Accesspoint>;
  public accesspointContainerSize: number;
  public accesspointSelected: Array<Accesspoint>;
  public page: number = 1
  public pageSize: number = 14;

  constructor(private cacheService: CacheManagerService, private selectedAccesspoint: SelectedAccesspointService) { }

  ngOnInit(): void {
    this.initializeDataTable();
    this.loadContent();
  }

  private changeSelectedAccesspoint(bssid: string): void {
    const accesspointByBssid = this.accesspointContainer.find(element => element.bssid == bssid);
    this.selectedAccesspoint.changeAccesspoint(accesspointByBssid);
  }

  private initializeDataTable(): void {
    this.accesspointContainer = this.cacheService.load(environment.CACHE_ACCESSPOINTS);
    this.accesspointContainerSize = this.accesspointContainer.length;
  }

  public loadContent(): void {
    this.accesspointSelected = this.accesspointContainer
      .map((accesspoint, i) => ({tid: i + 1, secName: this.getSecurityName(accesspoint), secColor: this.getSecurityColor(accesspoint), ...accesspoint}))
      .slice((this.page - 1) * this.pageSize, (this.page - 1) * this.pageSize + this.pageSize);
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
