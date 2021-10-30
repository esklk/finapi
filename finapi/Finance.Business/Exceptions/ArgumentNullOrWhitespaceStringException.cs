﻿using System;

namespace Finance.Business.Exceptions
{
    class ArgumentNullOrWhitespaceStringException : ArgumentException
    {
        public ArgumentNullOrWhitespaceStringException(string paramName) : base("The value must not be null, empty or whitespace string.", paramName) { }
    }
}
