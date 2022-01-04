import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { TeamService } from 'src/app/services';
import { TeamViewDTO } from 'src/app/data-model';

@Component({
  selector: 'app-team-delete',
  templateUrl: './team-delete.component.html',
  styleUrls: ['./team-delete.component.css']
})
export class TeamDeleteComponent implements OnInit 
{
  // properties
  deleteTeam: TeamViewDTO;

  // fields
  private entityToDeleteId: number;

  constructor(
    private teamService: TeamService,
    private route: ActivatedRoute,
    private router: Router
  ) 
  {
    this.entityToDeleteId = +this.route.snapshot.paramMap.get("id");
  }

  ngOnInit() 
  {
    this.getSingle(this.entityToDeleteId);
  }

  // methods
  private getSingle(entityId: number) : void
  {
    this.teamService.getSingle<TeamViewDTO>(entityId)
        .subscribe(team => this.deleteTeam = team);
  }
  public delete(entityId: number) : void
  {
    this.teamService.delete(this.entityToDeleteId)
      .subscribe(data => this.router.navigateByUrl("/teams"));
  }
}