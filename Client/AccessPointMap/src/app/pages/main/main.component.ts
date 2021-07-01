import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable, Subscription } from 'rxjs';
import { AccessPoint } from 'src/app/core/models/access-point.model';
import { AccessPointService } from 'src/app/core/services/access-point.service';
import { AccessPointDetailsComponent } from 'src/app/shared/access-point-details/access-point-details.component';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {
  public accessPointsObservable: Observable<Array<AccessPoint>>;
  public mapId: string = 'mainMap';
  public mapHeight: string = '85vh';
  public zoom: number = 16;

  private urlParam: string;
  
  constructor(private modalService: NgbModal, private accessPointService: AccessPointService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    //Check if the page was opened with specified accesspoint id
    this.urlParam = this.route.snapshot.paramMap.get('id');
    if(this.urlParam != null) {
      this.showDetailsUrl(Number(this.urlParam));
    }

    //Continue with geting the ,,all accesspoints'' observable
    this.accessPointsObservable = this.accessPointService.getAllPublic(1);
  }

  //On marker click event show modal with details
  public showDetailsClick(accessPoints: Array<AccessPoint>): void {
    const ref = this.modalService.open(AccessPointDetailsComponent, { modalDialogClass: 'modal-xl'});

    if(Array.isArray(accessPoints)) {
      ref.componentInstance.accessPoints = accessPoints;
    } else {
      ref.componentInstance.accessPoints = [ accessPoints ];
    }
    
    const changesSubscription: Subscription = ref.componentInstance.changeEvent.subscribe((result : AccessPoint) => {
      this.accessPointsObservable = this.accessPointService.getAllPublic(1);
    });

    const deleteSubscription: Subscription = ref.componentInstance.deleteEvent.subscribe((res: AccessPoint) => {
      this.accessPointsObservable = this.accessPointService.getAllPublic(1);
    });

    ref.result.then(() => {
      changesSubscription.unsubscribe();
      deleteSubscription.unsubscribe();
    }, () => {});
  }

  //If the url has a accesspoint param show the deatils of accesspoint with given id
  private showDetailsUrl(accessPointId: number): void {
    this.accessPointService.getByIdPublic(accessPointId, 1).subscribe((res) => {
      const ref = this.modalService.open(AccessPointDetailsComponent, { modalDialogClass: 'modal-xl'});

      ref.componentInstance.accessPoints = [ res ];
      ref.result.then(() => {}, () => {});
    },
    (error) => {
      console.error(error);
    });
  }
}
