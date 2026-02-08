import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PageNotFoundComponent } from './page-not-found.component/page-not-found.component';

const routes: Routes = [
  {
    path: 'signIn',
    loadChildren: () => import('./sign-in/sign-in-module').then(m => m.SignInModule)
  },
  {
    path: 'dashboard',
    loadChildren: () => import('./dashboard/dashboard-module').then(m => m.DashboardModule)
  },
  { path: '', redirectTo: '/signIn', pathMatch: 'full' }, // ByDefault page open
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
