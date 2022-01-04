import { NgModule } from '@angular/core';

import { TeamService } from 'src/app/services';
import { SharedModule } from 'src/app/shared.module';
import { TeamsListComponent, TeamCreateComponent, TeamEditComponent, TeamDeleteComponent } from './components/index';

@NgModule({
  declarations: 
  [
    TeamsListComponent, TeamCreateComponent, TeamEditComponent, TeamDeleteComponent
  ],
  imports: 
  [
    SharedModule
  ],
  providers:
  [
    TeamService
  ]
})
export class TeamModule { }
