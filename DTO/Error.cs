using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Resulte<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }

        public static Resulte<T> Success(T data) => new Resulte<T> { IsSuccess = true, Data = data };
        public static Resulte<T> Failure(string error) => new Resulte<T> { IsSuccess = false, ErrorMessage = error };
    }
}
