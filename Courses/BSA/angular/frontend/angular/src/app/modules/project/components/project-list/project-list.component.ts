import { Component, OnInit } from '@angular/core';

import { ProjectService } from 'src/app/services';
import { ProjectListDTO } from 'src/app/data-model';

@Component({
  selector: 'app-project-list',
  templateUrl: './project-list.component.html',
  styleUrls: ['./project-list.component.css']
})
export class ProjectListComponent implements OnInit 
{
  // properties
  projects : ProjectListDTO[];

  // fields
  constructor(
    private projectService : ProjectService
  ) { }

  // initializers
  ngOnInit() 
  {
    this.getProjects();
  }
  private getProjects() : void
  {
    this.projectService.getAll<ProjectListDTO>()
      .subscribe(projects => this.projects = projects);
  }

}