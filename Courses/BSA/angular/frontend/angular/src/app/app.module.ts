import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NotifierModule } from 'angular-notifier';

import { AppComponent } from './app.component';
import { AppRoutes } from './app.routes';
import { UserModule, TeamModule, ProjectModule, TaskModule } from './modules/index';
import { LayoutComponent, HomeComponent, NavigationComponent, FooterComponent } from './components/index';

@NgModule({  
  declarations: 
  [
    AppComponent, LayoutComponent, HomeComponent, NavigationComponent, FooterComponent
  ],
  imports: 
  [
    BrowserModule, HttpClientModule, NotifierModule,
    UserModule, TeamModule, ProjectModule, TaskModule,
    RouterModule.forRoot(AppRoutes)
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
