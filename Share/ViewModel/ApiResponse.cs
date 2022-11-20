using Share.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.ViewModel
{
    public class ApiResponse
    {
        public ApiResponseCodes Code { get; set; }
        public int Result
        {
            get
            {
                return (int)Code;
            }
        }
        public string Description { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Payload { get; set; } = default;
        public int TotalCount { get; set; }
        public string ResponseCode { get; set; }
        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();
        public bool HasErrors => Errors.Any();
    }
}
