import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { LoginGuard } from './core/guards/login.guard';
import { ListComponent } from './pages/list/list.component';
import { LoginComponent } from './pages/login/login.component';
import { MainComponent } from './pages/main/main.component';
import { PanelComponent } from './pages/panel/panel.component';
import { SearchComponent } from './pages/search/search.component';
import { StatisticsComponent } from './pages/statistics/statistics.component';

const routes: Routes = [
  { path: '', component: MainComponent },
  { path: 'accesspoint/:id', component: MainComponent },
  { path: 'list', component: ListComponent },
  { path: 'list/accesspoint/:id', component: ListComponent },
  { path: 'search', component: SearchComponent },
  { path: 'statistics', component: StatisticsComponent },
  { path: 'login', component: LoginComponent, canActivate: [LoginGuard] },
  { path: 'panel', component: PanelComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
