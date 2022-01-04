import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { ProjectService } from 'src/app/services';
import { ProjectViewDTO } from 'src/app/data-model';

@Component({
  selector: 'app-project-delete',
  templateUrl: './project-delete.component.html',
  styleUrls: ['./project-delete.component.css']
})
export class ProjectDeleteComponent implements OnInit 
{
  // properties
  deleteItem : ProjectViewDTO;

  // fields
  private entityToDeleteId: number;

  constructor(    
    private projectService: ProjectService,
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
    this.projectService.getSingle<ProjectViewDTO>(entityId)
        .subscribe(project => this.deleteItem = project);
  }

  // methods
  public delete(entityId: number) : void
  {
    this.projectService.delete(this.entityToDeleteId)
      .subscribe(data => this.router.navigateByUrl("/projects"));
  }
}
