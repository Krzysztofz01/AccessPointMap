import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { AccessPoint } from 'src/app/core/models/access-point.model';
import { AccessPointService } from 'src/app/core/services/access-point.service';
import { AccessPointDeatilModalService } from 'src/app/shared/access-point-details/services/access-point-deatil-modal.service';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {
  public accessPointsObservable: Observable<Array<AccessPoint>>;
  public mapId: string = 'mainMap';
  public mapHeight: string = '75vh';
  public zoom: number = 16;

  private urlParam: string;

  constructor(private accessPointService: AccessPointService, private route: ActivatedRoute, private accessPointModal: AccessPointDeatilModalService) { }

  ngOnInit(): void {
    //Check if the page was opened with specified accesspoint id
    this.urlParam = this.route.snapshot.paramMap.get('id');
    if(this.urlParam != null) {
      this.showDetailsUrl(Number(this.urlParam));
    }

    //Continue with geting the ,,all accesspoints'' observable
    this.accessPointsObservable = this.accessPointService.getAllPublic(1);
  }

  public showDetailsClick(accessPoints: Array<AccessPoint>): void {
    this.accessPointModal.open(accessPoints);
  }

  private showDetailsUrl(accessPointId: number): void {
    this.accessPointService.getByIdPublic(accessPointId, 1).subscribe((res) => {
      this.accessPointModal.open(res);
    },
    (error) => {
      console.error(error);
    });
  }
}
