using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingMilitia.AkkaSampleApplication.Messages
{
    public class StuffCountRequestWrapper
    {
        public int StuffHandlerId { get; set; }
        public StuffCountRequest StuffCountRequest { get; set; }
    }
}
