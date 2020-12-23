import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PageAdminComponent } from './layout/page-admin/page-admin.component';
import { PageMainComponent } from './layout/page-main/page-main.component';

const routes: Routes = [
  { path: '', component: PageMainComponent },
  { path: 'admin', component: PageAdminComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
