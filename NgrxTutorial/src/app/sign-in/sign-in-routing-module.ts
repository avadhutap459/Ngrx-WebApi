import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SignInLayoutComponent } from './sign-in-layout.component/sign-in-layout.component';
import { SignInComponent } from './sign-in.component/sign-in.component';

const routes: Routes = [
  {path : '', component : SignInLayoutComponent,
    children :[
      {path : '' , component : SignInComponent}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SignInRoutingModule { }
