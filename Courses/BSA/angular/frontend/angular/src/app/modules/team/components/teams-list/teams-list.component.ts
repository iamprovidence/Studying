import { Component, OnInit } from '@angular/core';

import { TeamService } from 'src/app/services';
import { TeamListDTO } from 'src/app/data-model';

@Component({
  selector: 'app-teams-list',
  templateUrl: './teams-list.component.html',
  styleUrls: ['./teams-list.component.css']
})
export class TeamsListComponent implements OnInit 
{
  // properties
  teams: TeamListDTO[];

  // fields
  constructor(private teamService : TeamService) { }

  ngOnInit() 
  {
    this.getList();
  }

  // methods
  private getList() : void
  {
    this.teamService.getAll<TeamListDTO>()
      .subscribe(teams => this.teams = teams);
  }
}