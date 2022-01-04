import { Component, OnInit, HostListener, ViewChild, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { NotifierService } from 'angular-notifier';

import { RequestResult, CreateProjectDTO, UserViewDTO, TeamViewDTO } from 'src/app/data-model';
import { ProjectService, UserService, TeamService } from 'src/app/services';
import { ComponentCanDeactivate } from 'src/app/interfaces';


@Component({
  selector: 'app-project-create',
  templateUrl: './project-create.component.html',
  styleUrls: ['./project-create.component.css']
})
export class ProjectCreateComponent implements OnInit, OnDestroy, ComponentCanDeactivate
{
  // template variables
  @ViewChild('form', { static: false }) 
  form : NgForm;
  
  // properties
  createProjectDTO : CreateProjectDTO;

  teams : TeamViewDTO[];
  users : UserViewDTO[];

  // fields
  constructor(
    private projectService : ProjectService,
    private userService : UserService,
    private teamService : TeamService,
    private notifierService: NotifierService,
    private router: Router
  ) 
  {
    this.createProjectDTO = new CreateProjectDTO();
  }

  // initializers
  ngOnInit() 
  {
    this.loadTeams();
    this.loadUsers();
  }

  private loadTeams() : void
  {
    this.teamService.getAll<TeamViewDTO>()
      .subscribe(teams => this.teams = teams);
  }
  private loadUsers() : void
  {
    this.userService.getAll<UserViewDTO>()
      .subscribe(users => this.users = users).unsubscribe();
  }
  ngOnDestroy(): void
  {
    this.notifierService.hideAll();
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
    this.projectService.create(this.createProjectDTO)
      .subscribe(
        data  =>
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