using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using System.Linq;

namespace WebAPI.Filters
{
    internal class ValidationActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid == false)
            {
                string errorMsg = string.Join(" ", context.ModelState.Values.SelectMany(x => x.Errors.Select(p => p.ErrorMessage)).ToArray());

                context.Result = new BadRequestObjectResult(errorMsg);
            }
        }
    }
}
