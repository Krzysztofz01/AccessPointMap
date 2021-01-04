import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/auth/services/auth.service';
import { User } from 'src/app/models/user.model';
import { UserDataService } from 'src/app/services/user-data.service';

@Component({
  selector: 'admin-user',
  templateUrl: './admin-user.component.html',
  styleUrls: ['./admin-user.component.css']
})
export class AdminUserComponent implements OnInit {
  public userContainer: Array<User>;
  public searchQuery: string;
  public sortKey: string = 'id';
  public sortReverse: boolean = false;
  public page: number = 1;

  constructor(private userDataService: UserDataService, private authService: AuthService) { }

  ngOnInit(): void {
    const token = this.authService.getToken();
    this.userDataService.getAllUsers(token)
      .subscribe((response) => {
        this.userContainer = response;
      });
  }

  public search(): void {
    this.page = 1;
    if(this.searchQuery == "") {
      this.ngOnInit();
    } else {
      this.userContainer = this.userContainer.filter(x => {
        return x.email.toLocaleLowerCase().match(this.searchQuery.toLocaleLowerCase()) ||
          x.lastLoginIp.toLocaleLowerCase().match(this.searchQuery.toLocaleLowerCase());
      });
    }
  }

  public sort(key: string): void {
    this.sortKey = key;
    this.sortReverse = !this.sortReverse;
  }
}
