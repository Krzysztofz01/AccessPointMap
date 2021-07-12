import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/authentication/services/auth.service';
import { User } from 'src/app/core/models/user.model';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit {
  public users: Array<User>;

  public searchKeyword: string;
  public key: string = 'id';
  public reverse: boolean = false;
  public page: number = 1;
  public pageSize: number = 12;

  constructor(private userService: UserService, private authService: AuthService) { }

  ngOnInit(): void {
    this.initializeData();
  }

  //Fetching user data from server
  public initializeData(): void {
    this.users = new Array<User>();
    this.userService.getAll(1).subscribe((res) => {
      this.users = res;
    },
    (error) => {
      console.error(error);
    });
  }

  //Change user activation status
  public activation(user: User): void {
    this.userService.activateById(user.id, 1).subscribe(() => {
      this.users.find(x => x.id == user.id).isActivated = !this.users.find(x => x.id == user.id).isActivated;
    },
    (error) => {
      console.error(error);
    });
  }

  //Delete a specific user
  public delete(user: User): void {
    this.userService.deleteById(user.id, 1).subscribe(() => {
      const index = this.users.findIndex(x => x.id == user.id);
      if(index !== -1) this.users.splice(index, 1);
    },
    (error) => {
      console.error(error);
    });
  }

  //Table sort implementation
  public sort(key: string): void {
    this.key = key;
    this.reverse = !this.reverse;
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
      this.users = this.users.filter((res) => {
        return query(res.name, kw) || query(res.email, kw);
      });
    }
  }

  //Check if the current user has a admin role
  public isAdmin(): boolean {
    if(this.authService.userValue != null) {
      return this.authService.userValue.role == 'Admin';
    }
    return false;
  }
}
