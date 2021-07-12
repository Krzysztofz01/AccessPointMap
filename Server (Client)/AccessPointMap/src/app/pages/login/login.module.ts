import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { RegisterModalComponent } from './register-modal/register-modal.component';



@NgModule({
  declarations: [LoginComponent, RegisterModalComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgbModule,
    RouterModule,
    SharedModule
  ]
})
export class LoginModule { }
