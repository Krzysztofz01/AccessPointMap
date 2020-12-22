import { Component } from '@angular/core';
import { DataFetchService } from './services/data-fetch.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'AccessPointMap';

  constructor(private dataFetchService: DataFetchService) {
    this.dataFetchService.localDataCheck();
  }
}
