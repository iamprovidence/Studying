using BusinessLayer.Commands;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using System.Linq;

namespace WebApi.Filters
{
    public class ValidationActionFilterAttribute : ActionFilterAttribute
    {       
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid == false)
            {
                context.Result = new BadRequestObjectResult(new CommandResponse
                {
                    IsSucessed = false,
                    Message = string.Join(" ", context.ModelState.Values.SelectMany(x => x.Errors.Select(p => p.ErrorMessage)).ToList())
                });
                
            }
        }
    }
}
