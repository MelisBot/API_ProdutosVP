using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Result
    {
        public string ErrorMessage { get; set; }
        public bool Correct { get; set; }
        public Exception Ex { get; set; }
        public object Object { get; set; }
        public List<object> Objects { get; set; }
    }
}
