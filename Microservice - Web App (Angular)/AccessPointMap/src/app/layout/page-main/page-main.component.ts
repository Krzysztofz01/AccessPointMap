import { Component, OnInit } from '@angular/core';
import { NavToggler } from 'src/app/models/nav-toggler.model';

const innerComponents: Array<NavToggler> = [
  { name: 'map', visible: false },
  { name: 'table', visible: false },
  { name: 'chSec', visible: false },
  { name: 'chAre', visible: false },
  { name: 'chFre', visible: false },
  { name: 'chBra', visible: false }
];

@Component({
  selector: 'page-main',
  templateUrl: './page-main.component.html',
  styleUrls: ['./page-main.component.css']
})
export class PageMainComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
    this.toggleComp('map');
  }

  public isToggled(name: string): boolean {
    return innerComponents.find(x => x.name == name).visible;
  }

  public toggleComp(name: string): void {
    innerComponents.forEach(x => {
      x.visible = false;
    });
    innerComponents.find(x => x.name == name).visible = true;
  }
}
