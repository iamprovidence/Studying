import { Component, OnInit, ViewChild, HostListener, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { NotifierService } from 'angular-notifier';

import { ProjectService, UserService, TeamService } from 'src/app/services';
import { ProjectViewDTO, UpdateProjectDTO, UserViewDTO, TeamViewDTO, RequestResult } from 'src/app/data-model';
import { ComponentCanDeactivate } from 'src/app/interfaces';


@Component({
  selector: 'app-project-edit',
  templateUrl: './project-edit.component.html',
  styleUrls: ['./project-edit.component.css']
})
export class ProjectEditComponent implements OnInit, OnDestroy, ComponentCanDeactivate
{
  // template variables
  @ViewChild('form', { static: false }) 
  form : NgForm;
  
  // properties
  updateItemDTO : UpdateProjectDTO;
  
  teams : TeamViewDTO[];
  users : UserViewDTO[];

  // fields
  constructor(
    private projectService: ProjectService,
    private userService: UserService,
    private teamService: TeamService,
    private notifierService: NotifierService,
    private route: ActivatedRoute,
    private router: Router) { }

  // initializers
  ngOnInit() 
  {
    const entityId : number = +this.route.snapshot.paramMap.get("id");

    this.getSingle(entityId);
    this.loadTeams();
    this.loadUsers();
  }
  
  private getSingle(entityId: number) : void
  {
    this.projectService.getSingle<ProjectViewDTO>(entityId)
        .subscribe(entity => this.updateItemDTO = entity);
  }    
  private loadTeams() : void
  {
    this.teamService.getAll<TeamViewDTO>()
      .subscribe(teams => this.teams = teams);
  }
  private loadUsers() : void
  {
    this.userService.getAll<UserViewDTO>()
      .subscribe(users => this.users = users);
  }
  
  ngOnDestroy(): void 
  {
    this.notifierService.hideAll();
  }  

  // events
  @HostListener('window:beforeunload')
  canDeactivate(): boolean 
  {
    return this.form.dirty ? confirm("You have unsaved changes. Are you sure you want to exit?") : true;
  }

  // methods
  public update() : void
  {
    this.projectService.update(this.updateItemDTO)
      .subscribe(data =>
        {          
          this.form.reset();
          this.router.navigateByUrl("/projects");
        }, 
        err => 
        {
          this.notifierService.show({ type: "error", message: (err.error as RequestResult).message });
        });
  }
}