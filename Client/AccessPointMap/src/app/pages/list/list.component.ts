import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AccessPoint } from 'src/app/core/models/access-point.model';
import { AccessPointService } from 'src/app/core/services/access-point.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {
  public accessPointsObservable: Observable<Array<AccessPoint>>

  constructor(private accessPointService: AccessPointService) { }

  ngOnInit(): void {
    this.accessPointsObservable = this.accessPointService.getAllPublic(1);
  }
}
