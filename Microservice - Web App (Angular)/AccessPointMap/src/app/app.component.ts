import { Component } from '@angular/core';
import { Router, Event, NavigationStart, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'AccessPointMap';
  public showLoadingIndicator: boolean = true;

  constructor(private router: Router) {
    this.router.events.subscribe((routerEvent: Event) => {
      if(routerEvent instanceof NavigationStart) {
        this.showLoadingIndicator = true;
      }

      if(routerEvent instanceof NavigationEnd) {
        this.showLoadingIndicator = false;
      }
    });
  }
}
