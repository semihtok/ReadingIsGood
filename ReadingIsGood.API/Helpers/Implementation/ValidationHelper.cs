using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ReadingIsGood.API.Helpers.Implementation
{
    public class ValidationHelper : IValidationHelper
    {
        public List<string> GetValidationErrors(ModelStateDictionary modelStateDictionary)
        {
            // var errors = ModelState.Select(x => x.Value.Errors)
            //     .Where(y => y.Count > 0)
            //     .ToList();
            //
            // foreach (var error in errors)
            // {
            //     response.Errors.Add(error.ToString());
            // }
            return new List<string>();
        }
    }
}