import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthRequestRegister } from 'src/app/authentication/models/auth-request-register.model';

@Component({
  selector: 'app-register-modal',
  templateUrl: './register-modal.component.html',
  styleUrls: ['./register-modal.component.css']
})
export class RegisterModalComponent implements OnInit {
  public registerForm: FormGroup;

  public notificationShow: boolean;
  public notificationText: string;
  public notificationType: string = 'danger';

  constructor(private modal: NgbActiveModal) { }

  ngOnInit(): void {
    this.notificationShow = false;

    this.registerForm = new FormGroup({
      email: new FormControl('', [ Validators.required, Validators.email ]),
      name: new FormControl('', [ Validators.required ]),
      password: new FormControl('', [ Validators.required, Validators.minLength(8) ]),
      repeatPassword: new FormControl('', [ Validators.required, Validators.minLength(8) ])
    });
  }

  private comparePassword(): boolean {
    return this.registerForm.get('password').value == this.registerForm.get('repeatPassword').value;
  }

  public submit(): void {
    if(this.registerForm.valid && this.comparePassword()) {
      const registerData: AuthRequestRegister = {
        email: this.registerForm.get('email').value,
        name: this.registerForm.get('name').value,
        password: this.registerForm.get('password').value,
        passwordRepeat: this.registerForm.get('repeatPassword').value
      }

      this.modal.close(registerData);
    } else {
      this.notificationShow = true;
      this.notificationText = 'Provided data invalid!';
    }
  }
}
