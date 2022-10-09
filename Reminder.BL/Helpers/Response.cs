using System.Collections.Generic;

namespace Reminder.BL.Helpers
{
    public class Response<T>
    {
        public bool Success { get; set; } = true;
        public T Data { get; set; }
        public string Message { get; set; }
        public int? Count { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
