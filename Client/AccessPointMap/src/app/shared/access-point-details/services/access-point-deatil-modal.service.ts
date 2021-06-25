import { Injectable } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AccessPoint } from 'src/app/core/models/access-point.model';
import { AccessPointDetailsComponent } from '../access-point-details.component';

@Injectable({
  providedIn: 'root'
})
export class AccessPointDeatilModalService {

  constructor(private modalSerivce: NgbModal) { }

  public open(accessPoint: AccessPoint | Array<AccessPoint>): any {
    const ref = this.modalSerivce.open(AccessPointDetailsComponent, { modalDialogClass: 'modal-xl'});

    if(Array.isArray(accessPoint)) {
      ref.componentInstance.accessPoints = accessPoint;
    } else {
      ref.componentInstance.accessPoints = [ accessPoint ];
    }

    ref.result.then((result) => {
      return result;
    },
    () => {});
  }
}
