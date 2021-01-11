import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PageAdminGuard } from './auth/guards/page-admin.guard';
import { PageAuthGuard } from './auth/guards/page-auth.guard';
import { PageUserGuard } from './auth/guards/page-user.guard';
import { PageAdminComponent } from './layout/page-admin/page-admin.component';
import { PageAuthComponent } from './layout/page-auth/page-auth.component';
import { PageErrorComponent } from './layout/page-error/page-error.component';
import { PageMainComponent } from './layout/page-main/page-main.component';
import { PageUserComponent } from './layout/page-user/page-user.component';

const routes: Routes = [
  { path: '', component: PageMainComponent },
  { path: 'auth', component: PageAuthComponent, canActivate: [PageAuthGuard] },
  { path: 'admin', component: PageAdminComponent, canActivate: [PageAdminGuard], canLoad: [PageAdminGuard] },
  { path: 'user', component: PageUserComponent, canActivate: [PageUserGuard], canLoad: [PageUserGuard] },
  { path: 'error', component: PageErrorComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
