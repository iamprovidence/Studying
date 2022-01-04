import { Component, OnInit } from '@angular/core';

import { TaskListDTO } from 'src/app/data-model';
import { TaskService } from 'src/app/services';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit 
{
  // properties
  tasks : TaskListDTO[];

  // fields
  constructor(
    private tasksService : TaskService
  ) { }

  // initializers
  ngOnInit() 
  {
    this.getTasks();
  }
  private getTasks() : void
  {
    this.tasksService.getAll<TaskListDTO>()
      .subscribe(tasks => this.tasks = tasks);
  }
}