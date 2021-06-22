import { APP_INITIALIZER, NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { appInitializer } from './initializers/app.initializer';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { PermissionErrorInterceptor } from './interceptors/permission-error.interceptor';
import { AuthService } from '../authentication/services/auth.service';
import { AccessPointService } from './services/access-point.service';
import { UserService } from './services/user.service';
import { DateParserService } from './services/date-parser.service';
import { LocalStorageService } from './services/local-storage.service';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [
    { provide: APP_INITIALIZER, useFactory: appInitializer, multi: true, deps: [AuthService]},
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: PermissionErrorInterceptor, multi: true },

    AccessPointService,
    UserService,
    DateParserService,
    LocalStorageService
  ]
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    if(parentModule) {
      throw new Error('Core module is already created! Only one instance should exist!');
    }
  }
}
