import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/authentication/services/auth.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
  public resetForm: FormGroup;

  public showNot: boolean;
  public typeNot: string;
  public textNot: string;

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.showNot = false;

    this.resetForm = new FormGroup({
      password: new FormControl('', [ Validators.required, Validators.minLength(8) ]),
      passwordRepeat: new FormControl('', [ Validators.required, Validators.minLength(8) ])
    });
  }

  private validatePassword(): boolean {
    return this.resetForm.get('password').value == this.resetForm.get('passwordRepeat').value;
  }

  public resetSubmit(): void {
    if(this.resetForm.valid && this.validatePassword()) {
      this.authService.resetPassword({
        password: this.resetForm.get('password').value,
        passwordRepeat: this.resetForm.get('passwordRepeat').value
      }).subscribe(() => {
        this.showNot = true;
        this.typeNot = 'success';
        this.textNot = 'Password changed';
      },
      (error) => {
        console.error(error);
        this.showNot = true;
        this.typeNot = 'danger';
        this.textNot = 'Server error, invalid data.';
      });
    } else {
      this.showNot = true;
      this.typeNot = 'danger';
      this.textNot = 'Invalid data.';
    }
    this.resetForm.reset();
  }

}
