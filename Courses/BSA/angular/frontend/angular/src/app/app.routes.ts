import { Routes } from '@angular/router';

import { HomeComponent } from './components';
import { UsersListComponent, UserViewComponent, UserCreateComponent, UserEditComponent, UserDeleteComponent } from './modules/user/components'
import { TeamsListComponent, TeamCreateComponent, TeamEditComponent, TeamDeleteComponent } from './modules/team/components';
import { TaskCreateComponent, TaskDeleteComponent, TaskEditComponent, TaskListComponent, TaskViewComponent} from './modules/task/components';
import { ProjectCreateComponent, ProjectDeleteComponent, ProjectEditComponent, ProjectViewComponent, ProjectListComponent  } from './modules/project/components';
import { ExitGuard } from './guards';

export const AppRoutes: Routes = 
[
    // home
    { path: '', pathMatch: "full", component: HomeComponent },

    // users
    { path: 'users',                component: UsersListComponent },
    { path: 'users/create',         component: UserCreateComponent, canDeactivate: [ ExitGuard ] },
    { path: 'users/view/:id',       component: UserViewComponent },
    { path: 'users/edit/:id',       component: UserEditComponent, canDeactivate: [ ExitGuard ] },
    { path: 'users/delete/:id',     component: UserDeleteComponent },

    // teams
    { path: 'teams',                component: TeamsListComponent },
    { path: 'teams/create',         component: TeamCreateComponent, canDeactivate: [ ExitGuard ] },
    { path: 'teams/edit/:id',       component: TeamEditComponent, canDeactivate: [ ExitGuard ] },
    { path: 'teams/delete/:id',     component: TeamDeleteComponent },

    // projects
    { path: 'projects',             component: ProjectListComponent },
    { path: 'projects/create',      component: ProjectCreateComponent, canDeactivate: [ ExitGuard ] },
    { path: 'projects/view/:id',    component: ProjectViewComponent },
    { path: 'projects/edit/:id',    component: ProjectEditComponent, canDeactivate: [ ExitGuard ] },
    { path: 'projects/delete/:id',  component: ProjectDeleteComponent },

    // tasks
    { path: 'tasks',                component: TaskListComponent },
    { path: 'tasks/create',         component: TaskCreateComponent, canDeactivate: [ ExitGuard ] },
    { path: 'tasks/view/:id',       component: TaskViewComponent },
    { path: 'tasks/edit/:id',       component: TaskEditComponent, canDeactivate: [ ExitGuard ] },
    { path: 'tasks/delete/:id',     component: TaskDeleteComponent },

    // any
    { path: '**', pathMatch: "full", redirectTo: '' }
];