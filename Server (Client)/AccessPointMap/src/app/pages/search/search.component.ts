import { Component, OnInit } from '@angular/core';
import { FormControl, FormControlName, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccessPoint } from 'src/app/core/models/access-point.model';
import { AccessPointService } from 'src/app/core/services/access-point.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  public searchForm: FormGroup;
  
  public searchDisabled: boolean = true;
  public tooMany: boolean = false;
  public empty: boolean = true;
  public results: boolean = false;

  public accessPoints: Array<AccessPoint>;

  constructor(private accessPointService: AccessPointService, private router: Router) { }

  ngOnInit(): void {
    this.searchForm = new FormGroup({
      ssid: new FormControl('', [ Validators.required, Validators.minLength(5) ])
    });

    this.searchForm.get('ssid').valueChanges.subscribe(() => {
      if(this.searchForm.get('ssid').valid) {
        this.searchDisabled = false;
      } else {
        this.searchDisabled = true;
      }
    });
  }

  public search(): void {
    if(this.searchForm.valid) {
      this.accessPoints = new Array<AccessPoint>();
      this.accessPointService.searchBySsid(this.searchForm.get('ssid').value, 1).subscribe((res) => {
        this.results = true;
        this.empty = true;
        this.tooMany = false;
        
        if(res.length > 0) this.empty = false
        if(res.length > 8) this.tooMany = true;
        
        this.accessPoints = res;
      },
      (error) => {
        console.error(error);
      })
    } else {
      console.log('Input data invalid!');
    }
  }

  public navigateToDetails(accesspoint: AccessPoint): void {
    this.router.navigate(['/accesspoint', String(accesspoint.id)]);
  }

  public securityStatusText(accessPoint: AccessPoint): string {
    const sd: Array<string> = JSON.parse(accessPoint.serializedSecurityData);

    if(sd.includes('WPA3')) return 'Secure';
    if(sd.includes('WPA2')) return 'Secure';
    if(sd.includes('WPA')) return 'Secure';
    if(sd.includes('WPS')) return 'Warning';
    if(sd.includes('WEP')) return 'Warning';
    return 'Danger';
  } 

  public securityStatusColor(accessPoint: AccessPoint): string {
    const sd: Array<string> = JSON.parse(accessPoint.serializedSecurityData);

    if(sd.includes('WPA3')) return 'var(--apm-success)';
    if(sd.includes('WPA2')) return 'var(--apm-success)';
    if(sd.includes('WPA')) return 'var(--apm-success)';
    if(sd.includes('WPS')) return 'var(--apm-warning)';
    if(sd.includes('WEP')) return 'var(--apm-warning)';
    return 'var(--apm-danger)';
  }

}
