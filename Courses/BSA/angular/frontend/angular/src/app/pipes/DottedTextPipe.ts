import { Pipe, PipeTransform } from '@angular/core';
 
@Pipe({
    name: 'dotted'
})
export class DottedTextPipe implements PipeTransform 
{
  public transform(value: string) : string 
  {
    return `${value}...`;
  }
  
}