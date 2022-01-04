import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { ProjectViewDTO } from 'src/app/data-model';
import { ProjectService } from 'src/app/services';


@Component({
  selector: 'app-project-view',
  templateUrl: './project-view.component.html',
  styleUrls: ['./project-view.component.css']
})
export class ProjectViewComponent implements OnInit 
{
  // properties
  project : ProjectViewDTO;

  // fields
  constructor(
    private projectService : ProjectService,
    private route: ActivatedRoute
  ) { }

  // initializers
  ngOnInit() 
  {
    const projectId: number = +this.route.snapshot.paramMap.get("id");
    this.getProject(projectId);
  }
  
  private getProject(projectId: number) : void
  {
    this.projectService.getSingle<ProjectViewDTO>(projectId)
        .subscribe(project => this.project = project);
  }
}