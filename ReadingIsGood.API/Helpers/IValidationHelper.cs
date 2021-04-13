using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ReadingIsGood.API.Helpers
{
    public interface IValidationHelper
    {
        List<string> GetValidationErrors(ModelStateDictionary modelStateDictionary);
    }
}