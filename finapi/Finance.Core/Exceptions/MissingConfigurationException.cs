using System;
using System.Reflection;

namespace Finance.Core.Exceptions
{
    public class MissingConfigurationException : Exception
    {
        public MissingConfigurationException(string key) : base($"Missing configuration: {key}.")
        {
            
        }
    }
}
