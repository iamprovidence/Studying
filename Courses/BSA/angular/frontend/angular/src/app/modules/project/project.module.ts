import { NgModule } from '@angular/core';

import { SharedModule } from 'src/app/shared.module';
import { ProjectCreateComponent, ProjectDeleteComponent, ProjectEditComponent, ProjectViewComponent, ProjectListComponent } from './components';
import { ProjectService } from 'src/app/services';

@NgModule({
  declarations: 
  [
    ProjectCreateComponent, ProjectDeleteComponent, ProjectEditComponent, ProjectViewComponent, ProjectListComponent
  ],
  imports: 
  [
    SharedModule
  ],
  providers:
  [
    ProjectService
  ]
})
export class ProjectModule { }
