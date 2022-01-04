import { NgModule } from '@angular/core';

import { SharedModule } from 'src/app/shared.module';
import { UserService, TeamService } from 'src/app/services';
import { UsersListComponent, UserEditComponent, UserViewComponent, UserDeleteComponent, UserCreateComponent } from './components';

@NgModule({
  declarations: 
  [
    UsersListComponent, UserViewComponent, UserEditComponent, UserDeleteComponent, UserCreateComponent
  ],
  imports: 
  [
    SharedModule
  ],
  providers:
  [
    UserService, TeamService
  ]
})
export class UserModule { }
