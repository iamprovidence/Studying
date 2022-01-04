import { Component, OnInit, HostListener, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';

import { CreateTeamDTO } from 'src/app/data-model';
import { TeamService } from 'src/app/services';
import { ComponentCanDeactivate } from 'src/app/interfaces';

@Component({
  selector: 'app-team-create',
  templateUrl: './team-create.component.html',
  styleUrls: ['./team-create.component.css']
})
export class TeamCreateComponent implements OnInit, ComponentCanDeactivate
{
  // template variables
  @ViewChild('form', { static: false }) 
  form : NgForm;

  // properties
  createTeamDTO : CreateTeamDTO;

  // fields
  constructor(
    private teamService: TeamService,  
    private router: Router)
    {
      this.createTeamDTO = new CreateTeamDTO();
    }

  ngOnInit() {
  }

  // events
  @HostListener('window:beforeunload')
  canDeactivate(): boolean 
  {
    return this.form.dirty ? confirm("You have unsaved changes. Are you sure you want to exit?") : true;
    //return !this.form.dirty || confirm("");
  }

  // methods
  public create() : void 
  {
    this.teamService.create(this.createTeamDTO)
      .subscribe(data => 
        {
          this.form.reset(); // reset dirty value, so do not show dialog on save
          this.router.navigateByUrl("/teams");
        });
  }
}
