import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';

import { TeamService } from 'src/app/services';
import { UpdateTeamDTO, TeamViewDTO } from 'src/app/data-model';
import { ComponentCanDeactivate } from 'src/app/interfaces';

@Component({
  selector: 'app-team-edit',
  templateUrl: './team-edit.component.html',
  styleUrls: ['./team-edit.component.css']
})
export class TeamEditComponent implements OnInit, ComponentCanDeactivate 
{
  // template variables
  @ViewChild('form', { static: false }) 
  form : NgForm;
  
  // properties
  updateTeam : UpdateTeamDTO;

  // fields
  private entityId : number;

  constructor(
    private teamService: TeamService,
    private route: ActivatedRoute,
    private router: Router) 
    {
      this.entityId = +this.route.snapshot.paramMap.get("id");
    }

  ngOnInit() 
  {
    this.getSingle(this.entityId);
  }
  
  // events
  @HostListener('window:beforeunload')
  canDeactivate() : boolean 
  {
    return this.form.dirty ? confirm("You have unsaved changes. Are you sure you want to exit?") : true;
  }  

  // methods
  private getSingle(entityId: number) : void
  {
    this.teamService.getSingle<TeamViewDTO>(entityId)
        .subscribe(team => this.updateTeam = team);
  }
  public update() : void
  {
    this.teamService.update(this.updateTeam)
      .subscribe(data =>
        {
          this.form.reset();
          this.router.navigateByUrl("/teams");
        });
  }
}

