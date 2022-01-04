import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';

import { CreateUserDTO, TeamViewDTO } from 'src/app/data-model';
import { UserService, TeamService } from 'src/app/services';
import { ComponentCanDeactivate } from 'src/app/interfaces';

@Component({
  selector: 'app-user-create',
  templateUrl: './user-create.component.html',
  styleUrls: ['./user-create.component.css']
})
export class UserCreateComponent implements OnInit, ComponentCanDeactivate
{
  // template variables
  @ViewChild('form', { static: false }) 
  form : NgForm;
  
  // properties
  createUserDTO : CreateUserDTO;

  teams : TeamViewDTO[];

  // fields
  constructor(
    private userService : UserService,
    private teamService : TeamService,
    private router: Router) 
    {
      this.createUserDTO = new CreateUserDTO();
    }

  ngOnInit() 
  {
    this.loadTeams();
  }

  // events
  @HostListener('window:beforeunload')
  canDeactivate() : boolean 
  {
    return this.form.dirty ? confirm("You have unsaved changes. Are you sure you want to exit?") : true;
  }  
  
  // methods
  private loadTeams() : void
  {
    this.teamService.getAll<TeamViewDTO>()
      .subscribe(teams => this.teams = teams);
  }

  public create() : void
  {
    this.userService.create(this.createUserDTO)
      .subscribe(data => 
        {
          this.form.reset();
          this.router.navigateByUrl("/users");
        });
  }
}