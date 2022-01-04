import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { UserService } from 'src/app/services';
import { UserViewDTO } from 'src/app/data-model';

@Component({
  selector: 'app-user-delete',
  templateUrl: './user-delete.component.html',
  styleUrls: ['./user-delete.component.css']
})
export class UserDeleteComponent implements OnInit 
{
  // properties
  deleteItem : UserViewDTO;

  // fields
  private entityToDeleteId: number;

  constructor(    
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router
  ) 
  {
    this.entityToDeleteId = +this.route.snapshot.paramMap.get("id");
  }

  ngOnInit() 
  {
    this.getSingle(this.entityToDeleteId)
  }

  // methods
  private getSingle(entityId: number) : void
  {
    this.userService.getSingle<UserViewDTO>(entityId)
        .subscribe(user => this.deleteItem = user);
  }
  public delete(entityId: number) : void
  {
    this.userService.delete(this.entityToDeleteId)
      .subscribe(data => this.router.navigateByUrl("/users"));
  }
}