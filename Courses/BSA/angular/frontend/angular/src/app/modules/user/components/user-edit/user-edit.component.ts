import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';

import { UserService, TeamService } from 'src/app/services';
import { UpdateUserDTO, UserViewDTO, TeamViewDTO } from 'src/app/data-model';
import { ComponentCanDeactivate } from 'src/app/interfaces';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit, ComponentCanDeactivate
{
  // template variables
  @ViewChild('form', { static: false }) 
  form : NgForm;
  
  // properties
  updateItemDTO : UpdateUserDTO;
  
  teams : TeamViewDTO[];

  // fields
  private entityId : number;

  constructor(
    private userService: UserService,
    private teamService: TeamService,
    private route: ActivatedRoute,
    private router: Router) 
  {
    this.entityId = +this.route.snapshot.paramMap.get("id");
  }

  ngOnInit() 
  {
    this.getSingle(this.entityId);
    this.loadTeams();
  }

  private getSingle(entityId: number) : void
  {
    this.userService.getSingle<UserViewDTO>(entityId)
        .subscribe(entity => this.updateItemDTO = entity);
  }    
  private loadTeams() : void
  {
    this.teamService.getAll<TeamViewDTO>()
      .subscribe(teams => this.teams = teams);
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
    this.userService.update(this.updateItemDTO)
      .subscribe(data =>
        {
          this.form.reset();
          this.router.navigateByUrl("/users");
        });
  }
}