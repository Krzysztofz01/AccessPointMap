import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/auth/services/auth.service';
import { Log } from 'src/app/models/log.model';
import { LogDataService } from 'src/app/services/log-data.service';

@Component({
  selector: 'admin-logs',
  templateUrl: './admin-logs.component.html',
  styleUrls: ['./admin-logs.component.css']
})
export class AdminLogsComponent implements OnInit {
  public logContainer: Array<Log>;
  public page: number = 1;

  constructor(private logDataService: LogDataService, private authService: AuthService) { }

  ngOnInit(): void {
    this.logDataService.getLogs(this.authService.getToken())
      .subscribe((response) => {
        this.logContainer = response;
      });
  }

  public formatDates(log: Log): string {
    const dt = new Date(log.date);
    return `${dt.getDay()}.${dt.getMonth()}.${dt.getFullYear()} ${dt.getHours()}:${dt.getMinutes()}:${dt.getSeconds()}`;
  }
}
