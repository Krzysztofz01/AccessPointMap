import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from 'src/app/auth/services/auth.service';
import { User } from 'src/app/models/user.model';
import { UserDataService } from 'src/app/services/user-data.service';
import { AdminUserEditModalComponent } from '../admin-user-edit-modal/admin-user-edit-modal.component';

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
  private token: string;

  constructor(private userDataService: UserDataService, private authService: AuthService, private modalService: NgbModal) { }

  ngOnInit(): void {
    this.token = this.authService.getToken();
    this.userDataService.getAllUsers(this.token)
      .subscribe((response) => {
        this.userContainer = response;
      });
  }

  public changeUserActivation(user: User): void {
    this.userDataService.activateUser(user.id, !user.active, this.token)
      .subscribe((response) => {
        console.log(response);
      });
  }

  public editUser(user: User): void {
    const ref = this.modalService.open(AdminUserEditModalComponent, { windowClass: 'custom-modal' });
    ref.componentInstance.user = user;

    ref.result.then((user) => {
      this.userDataService.updateUser(user, this.token)
        .subscribe((response) => {
          console.log(response);
        });
    },
    (exit) => {
      console.log('No changes');
    });
  }

  public deleteUser(user: User): void {
    this.userDataService.deleteUser(user.id, this.token)
      .subscribe((response) => {
        console.log(response);
      });
  }

  public dateFormat(date: Date): string {
    const dt = new Date(date);
    return `${dt.getHours()}:${dt.getMinutes()} ${dt.getDay()}.${dt.getMonth()}.${dt.getFullYear()}`;
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
