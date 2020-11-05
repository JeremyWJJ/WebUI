using System;

namespace SpdCommon.Modules
{
    public class CoreException : Exception
    {
        public int Code { get; set; }

        public CoreException(string msg = "", Exception innserException = null) : base(msg, innserException)
        {
        }
    }

    public class CoreCommonException : CoreException
    {
        public CoreCommonException(string msg, int code = 10000) : base(msg)
        {
            Code = code;
        }
    }

    public class CoreAuthException : CoreException
    {
        public CoreAuthException(string msg, int code = 20000) : base(msg)
        {
            Code = code;
        }
    }
}