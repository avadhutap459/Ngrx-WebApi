import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DashboardRoutingModule } from './dashboard-routing-module';
import { DashboardLayoutComponent } from './dashboard-layout.component/dashboard-layout.component';
import { DashboardComponent } from './dashboard.component/dashboard.component';


@NgModule({
  declarations: [
    DashboardLayoutComponent,
    DashboardComponent
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule
  ]
})
export class DashboardModule { }
