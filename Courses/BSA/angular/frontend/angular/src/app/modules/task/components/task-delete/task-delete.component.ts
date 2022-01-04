import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { TaskService } from 'src/app/services';
import { TaskViewDTO } from 'src/app/data-model';

@Component({
  selector: 'app-task-delete',
  templateUrl: './task-delete.component.html',
  styleUrls: ['./task-delete.component.css']
})
export class TaskDeleteComponent implements OnInit 
{
  // properties
  deleteItem : TaskViewDTO;

  // fields
  private entityToDeleteId: number;

  constructor(    
    private taskService: TaskService,
    private route: ActivatedRoute,
    private router: Router
  ) 
  {
    this.entityToDeleteId = +this.route.snapshot.paramMap.get("id");
  }

  // initializers
  ngOnInit() 
  {
    this.getSingle(this.entityToDeleteId)
  }

  private getSingle(entityId: number) : void
  {
    this.taskService.getSingle<TaskViewDTO>(entityId)
        .subscribe(task => this.deleteItem = task);
  }

  // methods
  public delete(entityId: number) : void
  {
    this.taskService.delete(this.entityToDeleteId)
      .subscribe(data => this.router.navigateByUrl("/tasks"));
  }
}