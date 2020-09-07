using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionControl.Exception
{
    public class NotAllowedValueException : ApplicationException
    {
        public NotAllowedValueException(string poruka) 
        { 
            ExtraMessage = poruka;
        }
        public string ExtraMessage { get; set; }
    }
}
