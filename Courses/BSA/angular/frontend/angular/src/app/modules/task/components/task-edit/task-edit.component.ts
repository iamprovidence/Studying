import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';

import { UserListDTO, ProjectListDTO, TaskStateListDTO, UpdateTaskDTO, TaskViewDTO } from 'src/app/data-model';
import { TaskService, TaskStateService, ProjectService, UserService } from 'src/app/services';
import { ComponentCanDeactivate } from 'src/app/interfaces';

@Component({
  selector: 'app-task-edit',
  templateUrl: './task-edit.component.html',
  styleUrls: ['./task-edit.component.css']
})
export class TaskEditComponent implements OnInit, ComponentCanDeactivate
{
  // template variables
  @ViewChild('form', { static: false }) 
  form : NgForm;
  
  // propertes  
  updateItemDTO : UpdateTaskDTO;

  taskStates : TaskStateListDTO[];
  projects : ProjectListDTO[];
  users : UserListDTO[];

  // fields
  constructor(
    private taskService : TaskService,
    private taskStateService : TaskStateService,
    private projectService : ProjectService,
    private userService : UserService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  // initializers
  ngOnInit() 
  {
    const entityId : number = +this.route.snapshot.paramMap.get("id");

    this.getSingle(entityId);
    this.loadProjects();
    this.loadUsers();
    this.loadTaskStates();
  }
  
  private getSingle(entityId: number) : void
  {
    this.taskService.getSingle<TaskViewDTO>(entityId)
        .subscribe(entity => this.updateItemDTO = entity);
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
  public update() : void
  {
    this.taskService.update(this.updateItemDTO)
      .subscribe(data =>
        {
          this.form.reset();
          this.router.navigateByUrl("/tasks");
        });
  }
}
