import { Component, Input, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Accesspoint } from 'src/app/models/accesspoint.model';
import { AccesspointDataService } from 'src/app/services/accesspoint-data.service';
import { AccesspointViewModalComponent } from '../accesspoint-view-modal/accesspoint-view-modal.component';

@Component({
  selector: 'app-page-check',
  templateUrl: './page-check.component.html',
  styleUrls: ['./page-check.component.css']
})
export class PageCheckComponent implements OnInit {
  @Input() ssidInput: string;
  public accesspointContainer: Array<Accesspoint>;
  public accesspointsNotFound: boolean;
  public accesspointToMany: boolean;

  constructor(private accesspointData: AccesspointDataService, private modalService: NgbModal) { }

  ngOnInit(): void {
    this.ssidInput = "";
    this.accesspointContainer = [];
    this.accesspointsNotFound = false;
    this.accesspointToMany = false;
  }

  public search(): void {
    this.accesspointData.searchBySsid(this.ssidInput.trim())
      .subscribe((response) => {
        if(response.length > 0) {
          if(response.length > 11) {
            this.accesspointContainer = [];
            this.accesspointsNotFound = true;
            this.accesspointToMany = true;
          } else {
            this.accesspointContainer = response;
            this.accesspointsNotFound = false;
            this.accesspointToMany = false;
          }
        } else {
          this.accesspointContainer = [];
          this.accesspointsNotFound = true;
          this.accesspointToMany = false;
        } 
      },
      (error) => {
        this.accesspointContainer = [];
        this.accesspointsNotFound = true;
        this.accesspointToMany = false;
      });
  }

  public getSecurityColor(accesspoint: Accesspoint): string {
    const securityArray: Array<string> = JSON.parse(accesspoint.securityData);

    if(securityArray.includes('WPA2') || securityArray.includes('WPA')) return 'security-good';
    if(securityArray.includes('WPS') || securityArray.includes('WEP')) return 'security-average';
    return 'security-bad';
  }

  public getSecurityName(accesspoint: Accesspoint): string {
    const securityArray: Array<string> = JSON.parse(accesspoint.securityData);

    if(securityArray.includes('WPA2')) return 'WPA2';
    if(securityArray.includes('WPA')) return 'WPA';
    if(securityArray.includes('WPS')) return 'WPS';
    if(securityArray.includes('WEP')) return 'WEP';
    return 'None';
  }

  public viewAccesspoint(accesspoint: Accesspoint): void {
    const ref = this.modalService.open(AccesspointViewModalComponent, { size: 'lg' });
    ref.componentInstance.mapInit(accesspoint);
  }

}
