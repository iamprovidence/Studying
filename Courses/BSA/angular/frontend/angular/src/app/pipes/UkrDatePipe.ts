import { Pipe, PipeTransform } from '@angular/core';
 
@Pipe({
    name: 'ukr_date'
})
export class UkrDatePipe implements PipeTransform 
{
  public transform(value: Date) : string 
  {
    if(value) 
    {
      // get string, box it to Date
      value = new Date(value);

      return `${value.getDate()} ${this.getMonth(value.getMonth())} ${value.getFullYear()}`;
    }
    return "";
  }
  private getMonth(value: number) : string
  {
    switch (value)
    {
      case 0: return "січень";
      case 1: return "лютий";
      case 2: return "березень";
      case 3: return "квітень";
      case 4: return "травень";
      case 5: return "червень";
      case 6: return "липень";
      case 7: return "серпень";
      case 8: return "вересень";
      case 9: return "жовтень";
      case 10: return "листопад";
      case 11: return "грудень";
    }
  }
}