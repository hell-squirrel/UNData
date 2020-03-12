using System.Collections.Generic;

namespace AppService.Models
{
    public class Result<T>
    {
        public T Data { get; set; }

        public bool IsSuccess { get; set; } = true;

        public string Error { get; set; }
    }
}