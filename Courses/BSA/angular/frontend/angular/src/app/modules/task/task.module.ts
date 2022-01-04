import { NgModule } from '@angular/core';

import { SharedModule } from 'src/app/shared.module';
import { TaskCreateComponent, TaskDeleteComponent, TaskEditComponent, TaskListComponent, TaskViewComponent } from './components';
import { TaskService, ProjectService, UserService, TaskStateService } from 'src/app/services';
import { TaskStateDirective } from 'src/app/directives';

@NgModule({
  declarations: 
  [
    TaskCreateComponent, TaskDeleteComponent, TaskEditComponent, TaskListComponent, TaskViewComponent,
    TaskStateDirective
  ],
  imports: 
  [
    SharedModule
  ],
  providers:
  [
    TaskService, TaskStateService, ProjectService, UserService
  ]
})
export class TaskModule { }
