import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from 'src/app/authentication/services/auth.service';
import { AccessPoint } from 'src/app/core/models/access-point.model';
import { AccessPointService } from 'src/app/core/services/access-point.service';
import { DateParserService } from 'src/app/core/services/date-parser.service';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-user-stat',
  templateUrl: './user-stat.component.html',
  styleUrls: ['./user-stat.component.css']
})
export class UserStatComponent implements OnInit {
  public aps: Array<AccessPoint>;
  public searchKeyword: string;
  public key: string = 'id';
  public reverse: boolean = false;
  public page: number = 1;
  public pageSize: number = 17;

  private userId: number; 

  constructor(private authService: AuthService, private accessPointService: AccessPointService, private dateService: DateParserService) { }

  ngOnInit(): void {
    this.initializeData();
  }

  //Fetch data from server
  public initializeData(): void {
    this.aps = new Array<AccessPoint>();

    this.accessPointService.getUserAdded(1).subscribe((added) => {
      this.accessPointService.getUserModified(1).subscribe((modified) => {
        this.aps = added.concat(modified);
        this.aps.sort((a, b) => a.id - b.id);

        this.userId = this.authService.userValue.id;
      },
      (error) => {
        console.error(error);
      });
    },
    (error) => {
      console.error(error);
    })
  }

  //Search system implementation
  public search(): void {
    const query = (param: string, key: string) => {
      return (param != null) ? param.toLowerCase().trim().match(key) : ''.match(key);
    }

    if(this.searchKeyword == '') {
      this.initializeData();
    } else {
      const kw = this.searchKeyword.trim().toLocaleLowerCase();
      this.aps = this.aps.filter((res) => {
        return query(res.ssid, kw) || query(res.bssid, kw) || query(res.serializedSecurityData, kw);
      });
    }
  }

  //Table sort implementation
  public sort(key: string): void {
    this.key = key;
    this.reverse = !this.reverse;
  }

  //Parse data
  public dateParse(date: Date): string {
    return this.dateService.parseDate(date);
  }

  //Get the name of user that posted this accesspoint
  public userNameAdded(accessPoint: AccessPoint): string {
    return accessPoint.userAdded.name;
  }

  //Get the name of user that modified this accesspoint
  public userNameModified(accessPoint: AccessPoint): string {
    return accessPoint.userModified.name;
  }

  //Get the CSS class to mark the current user for added column
  public nameColorAdded(accessPoint: AccessPoint): string {
    if (accessPoint.userAdded.id == this.userId) return 'current-user-name';
    return '';
  }

  //Get the CSS class to mark the current user for modified column
  public nameColorModified(accessPoint: AccessPoint): string {
    if (accessPoint.userModified.id == this.userId) return 'current-user-name';
    return '';
  }
}