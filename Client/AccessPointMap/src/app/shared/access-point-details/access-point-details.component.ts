import { Component, OnInit } from '@angular/core';
import { AccessPoint } from 'src/app/core/models/access-point.model';

@Component({
  selector: 'app-access-point-details',
  templateUrl: './access-point-details.component.html',
  styleUrls: ['./access-point-details.component.css']
})
export class AccessPointDetailsComponent implements OnInit {
  public accessPoints: Array<AccessPoint>;

  constructor() { }

  ngOnInit(): void {
    console.log(this.accessPoints);
  }

}
