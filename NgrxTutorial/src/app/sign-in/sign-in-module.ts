import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SignInRoutingModule } from './sign-in-routing-module';
import { SignInLayoutComponent } from './sign-in-layout.component/sign-in-layout.component';
import { SignInComponent } from './sign-in.component/sign-in.component';
import { ReactiveFormsModule } from '@angular/forms';
import { UserService } from './services/user.service';
import { HttpClientModule } from '@angular/common/http';


@NgModule({
  declarations: [
    SignInLayoutComponent,
    SignInComponent
  ],
  imports: [
    HttpClientModule,
    CommonModule,
    SignInRoutingModule,
    ReactiveFormsModule
  ],
  providers:[
  ]
})
export class SignInModule { }
