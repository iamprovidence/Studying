import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { TaskViewDTO } from 'src/app/data-model';
import { TaskService } from 'src/app/services';

@Component({
  selector: 'app-task-view',
  templateUrl: './task-view.component.html',
  styleUrls: ['./task-view.component.css']
})
export class TaskViewComponent implements OnInit 
{
  // properties
  task : TaskViewDTO;

  // fields
  constructor(
    private taksService : TaskService,
    private route : ActivatedRoute
  ) { }

  // initializerss
  ngOnInit() 
  {
    const taskId: number = +this.route.snapshot.paramMap.get("id");
    this.getProject(taskId);
  }
  
  private getProject(taskId: number) : void
  {
    this.taksService.getSingle<TaskViewDTO>(taskId)
        .subscribe(task => this.task = task);
  }

}