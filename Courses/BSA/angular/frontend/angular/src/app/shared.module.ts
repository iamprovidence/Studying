import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule }   from '@angular/forms';

import { UkrDatePipe, DottedTextPipe } from './pipes';
import { ExitGuard } from './guards';

@NgModule({
  imports: 
  [
    CommonModule, RouterModule, FormsModule
  ],
  declarations:
  [
    UkrDatePipe, DottedTextPipe,
  ],
  providers:
  [
    ExitGuard
  ],
  exports:
  [
    CommonModule, RouterModule, FormsModule,
    UkrDatePipe, DottedTextPipe
  ]
})
export class SharedModule { }
