﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pay_Xpert.Exceptions
{
   
        public class InvalidInputException : Exception
        {
            public InvalidInputException(string message) : base(message) { }
        }
    
}
