import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';

import { CreateTaskDTO, UserListDTO, ProjectListDTO, TaskStateListDTO } from 'src/app/data-model';
import { TaskService, TaskStateService, ProjectService, UserService } from 'src/app/services';
import { ComponentCanDeactivate } from 'src/app/interfaces';

@Component({
  selector: 'app-task-create',
  templateUrl: './task-create.component.html',
  styleUrls: ['./task-create.component.css']
})
export class TaskCreateComponent implements OnInit, ComponentCanDeactivate
{  
  // template variables
  @ViewChild('form', { static: false }) 
  form : NgForm;
  
  // properties
  createTaskDTO : CreateTaskDTO;

  taskStates : TaskStateListDTO[];
  projects : ProjectListDTO[];
  users : UserListDTO[];

  // fields
  constructor(
    private taskService : TaskService,
    private taskStateService : TaskStateService,
    private projectService : ProjectService,
    private userService : UserService,
    private router: Router
  ) 
  {
    this.createTaskDTO = new CreateTaskDTO();
  }
  
  // initializers
  ngOnInit() 
  {
    this.loadTaskStates();
    this.loadProjects();
    this.loadUsers();
  }

  private loadProjects() : void
  {
    this.projectService.getAll<ProjectListDTO>()
      .subscribe(projects => this.projects = projects);
  }  
  private loadTaskStates() : void
  {
    this.taskStateService.getAll()
      .subscribe(taskStates => this.taskStates = taskStates);
  }
  private loadUsers() : void
  {
    this.userService.getAll<UserListDTO>()
      .subscribe(users => this.users = users);
  }
  
  // events
  @HostListener('window:beforeunload')
  canDeactivate() : boolean 
  {
    return this.form.dirty ? confirm("You have unsaved changes. Are you sure you want to exit?") : true;
  }  
  
  // methods
  public create() : void
  {
    this.taskService.create(this.createTaskDTO)
      .subscribe(data => 
        {
          this.form.reset();
          this.router.navigateByUrl("/tasks");
        });
  }
}
