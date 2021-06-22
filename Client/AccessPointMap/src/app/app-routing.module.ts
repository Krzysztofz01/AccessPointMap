import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ListComponent } from './pages/list/list.component';
import { MainComponent } from './pages/main/main.component';
import { SearchComponent } from './pages/search/search.component';
import { StatisticsComponent } from './pages/statistics/statistics.component';

const routes: Routes = [
  { path: '', component: MainComponent },
  { path: 'accesspoint/:id', component: MainComponent },
  { path: 'list', component: ListComponent },
  { path: 'list/accesspoint/:id', component: ListComponent },
  { path: 'search', component: SearchComponent },
  { path: 'statistics', component: StatisticsComponent },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
