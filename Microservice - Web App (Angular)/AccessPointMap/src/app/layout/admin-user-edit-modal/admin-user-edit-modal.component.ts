import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { User } from 'src/app/models/user.model';

@Component({
  selector: 'admin-user-edit-modal',
  templateUrl: './admin-user-edit-modal.component.html',
  styleUrls: ['./admin-user-edit-modal.component.css']
})
export class AdminUserEditModalComponent implements OnInit {
  public user: User;
  public userForm: FormGroup;

  constructor(public modal: NgbActiveModal) { }

  ngOnInit(): void {
    this.userForm = new FormGroup({
      tokenExpiration: new FormControl(this.user.tokenExpiration, [Validators.required]),
      readPermission: new FormControl(this.user.readPermission),
      writePermission: new FormControl(this.user.writePermission),
      adminPermission: new FormControl(this.user.adminPermission)
    });
  }

  public saveChanges() {
    if(this.userForm.valid) {
      const values = this.userForm.value;
      this.user.tokenExpiration = values.tokenExpiration;
      this.user.readPermission = values.readPermission;
      this.user.writePermission = values.writePermission;
      this.user.adminPermission = values.adminPermission;

      this.modal.close(this.user);
    }
  }
}
