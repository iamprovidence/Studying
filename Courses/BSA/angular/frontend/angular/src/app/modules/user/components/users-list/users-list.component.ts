import { Component, OnInit } from '@angular/core';

import { UserService } from 'src/app/services';
import { UserListDTO } from 'src/app/data-model';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit 
{
  // properties
  users : UserListDTO[];

  // fields
  constructor(private userService : UserService) { }

  ngOnInit()
  {
    this.getUsers();
  }

  // methods
  private getUsers() : void
  {
    this.userService.getAll<UserListDTO>()
        .subscribe(users => this.users = users);
  }
}
