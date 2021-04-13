using System.Collections.Generic;
using System.Linq;

namespace ReadingIsGood.Domain
{
    public class BaseResponse<TData>
    {
        public bool HasError => Errors.Any();
        public List<string> Errors { get; set; }
        public int Total { get; set; }
        public TData Data { get; set; }
        public string Message { get; set; }

        public BaseResponse()
        {
            Errors = new List<string>();
        }
    }
}