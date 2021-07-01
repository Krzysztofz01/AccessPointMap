import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs';
import { AccessPoint } from 'src/app/core/models/access-point.model';
import { AccessPointService } from 'src/app/core/services/access-point.service';
import { AccessPointDetailsComponent } from 'src/app/shared/access-point-details/access-point-details.component';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {
  public accessPointsObservable: Observable<Array<AccessPoint>>
  
  private urlParam: string;

  constructor(private accessPointService: AccessPointService, private modalService: NgbModal, private route: ActivatedRoute) { }

  ngOnInit(): void {
    //Check if the page was opened with specified accesspoint id
    this.urlParam = this.route.snapshot.paramMap.get('id');
    if(this.urlParam != null) {
      this.showDetailsUrl(Number(this.urlParam));
    }

    //Continue with geting the ,,all accesspoints'' observable
    this.accessPointsObservable = this.accessPointService.getAllPublic(1);
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
