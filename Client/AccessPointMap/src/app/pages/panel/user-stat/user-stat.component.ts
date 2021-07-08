import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
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
  public pageSize: number = 12;

  constructor(private userService: UserService, private dateService: DateParserService, private modalService: NgbModal) { }

  ngOnInit(): void {
    this.initializeData();
  }

  //Fetch data from server
  public initializeData(): void {
    this.aps = new Array<AccessPoint>();
    this.userService.getCurrent(1).subscribe((res) => {
      this.aps = res.addedAccessPoints.concat(res.modifiedAccessPoints);
      
      this.aps.forEach(x => { 
        if(x == null) console.log(x)
      });

      // this.aps = this.aps.map((accessPoint) => ({ 
      //   usernameAdd: accessPoint.userAdded.name, 
      //   usernameEdit: accessPoint.userModified.name,
      //   ...accessPoint 
      // }));
    },
    (error) => {
      console.log(error);
    });
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
  
}
