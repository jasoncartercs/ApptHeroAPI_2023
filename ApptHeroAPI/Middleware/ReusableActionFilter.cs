namespace ApptHeroAPI.Middleware
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using ApptHeroAPI.Controllers;
    using System.Linq;

    public class ReusableActionFilter : IActionFilter
    {
        private readonly IEnumerable<string> _claimNames;

   

        public ReusableActionFilter(IEnumerable<string> claimNames)
        {
            _claimNames = claimNames;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as ReusableBaseController;
            if (controller != null)
            {
                foreach (var claimName in _claimNames)
                {
                    var claim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == claimName);
                    if (claim != null)
                    {
                        controller.ClaimsDictionary[claimName] = claim.Value;
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // You can also perform some operations after the action execution.
        }
    }

}
