import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { UserService } from 'src/app/services';
import { UserViewDTO } from 'src/app/data-model';

@Component({
  selector: 'app-user-view',
  templateUrl: './user-view.component.html',
  styleUrls: ['./user-view.component.css']
})
export class UserViewComponent implements OnInit 
{
  // properties
  user : UserViewDTO;

  // fields
  constructor(private userService : UserService,
              private route: ActivatedRoute) { }

  ngOnInit()
  {
    const userId: number = +this.route.snapshot.paramMap.get("id");

    this.getUser(userId);
  }

  // methods
  private getUser(userId: number) : void
  {
    this.userService.getSingle<UserViewDTO>(userId)
        .subscribe(user => this.user = user);
  }

}
