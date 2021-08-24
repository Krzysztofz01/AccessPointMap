import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthRequestRegister } from 'src/app/authentication/models/auth-request-register.model';
import { AuthService } from 'src/app/authentication/services/auth.service';
import { RegisterModalComponent } from './register-modal/register-modal.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  public loginForm: FormGroup;

  public notificationShow: boolean;
  public notificationText: string;
  public notificationType: string = 'danger';

  constructor(private authService: AuthService, private router: Router, private modalService: NgbModal) { }

  ngOnInit(): void {
    this.notificationShow = false;

    this.initializeRefreshToken();

    this.loginForm = new FormGroup({
      email: new FormControl('', [ Validators.required, Validators.email ]),
      password: new FormControl('', [ Validators.required, Validators.minLength(6) ])
    });
  }

  private initializeRefreshToken(): void {
    this.authService.refreshToken().subscribe(() => {
      this.router.navigate(['']);
    },
    (error) => {
      console.error(error);
    });
  }

  public loginSubmit(): void {
    this.notificationShow = false;
    if(this.loginForm.valid) {
      this.authService.login({
        email: this.loginForm.get('email').value,
        password: this.loginForm.get('password').value
      }).subscribe(() => {
        this.router.navigate(['']);
      },
      (error) => {
        console.error(error);
        this.notificationShow = true;
        this.notificationType = 'danger';
        this.notificationText = 'Check the provided data. Your account may not be activated yet!';
      });
    } else {
      this.notificationShow = true;
      this.notificationType = 'danger';
      this.notificationText = 'Provided data invalid!';
    }

    this.loginForm.reset();
  }

  public register(): void {
    const ref = this.modalService.open(RegisterModalComponent);

    ref.result.then(res => {
      const registerData = res as AuthRequestRegister;
      if (registerData != null) {
        this.authService.register(registerData).subscribe(() => {
          this.notificationShow = true;
          this.notificationType = 'success';
          this.notificationText = 'Registration successful. Wait for account activation.';
        },
        (error) => {
          console.error(error);
        });
      } else {
        this.notificationShow = true;
        this.notificationType = 'danger';
        this.notificationText = 'Provided data invalid!';
      }
    }, () => {});
  }
}
