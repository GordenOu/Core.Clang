namespace Core.Clang
{
    /// <summary>
    /// Error codes returned by libclang routines.
    /// </summary>
    [EnumMapping(typeof(CXErrorCode), Prefix = "CXError_")]
    public enum ErrorCode
    {
        /// <summary>
        /// No error.
        /// </summary>
        Success = 0,

        /// <summary>
        /// A generic error code, no further details are available.
        /// </summary>
        /// <remarks>
        /// Errors of this kind can get their own specific error codes in future libclang versions.
        /// </remarks>
        Failure = 1,

        /// <summary>
        /// libclang crashed while performing the requested operation.
        /// </summary>
        Crashed = 2,

        /// <summary>
        /// The function detected that the arguments violate the function contract.
        /// </summary>
        InvalidArguments = 3,

        /// <summary>
        /// An AST deserialization error has occurred.
        /// </summary>
        ASTReadError = 4
    }

    /// <summary>
    /// The exception that is thrown when the error code returned by a libclang routine is not
    /// <see cref="ErrorCode.Success"/>.
    /// </summary>
    public class ErrorCodeException : ClangException
    {
        /// <summary>
        /// The error code returned by libclang.
        /// </summary>
        public ErrorCode ErrorCode { get; }

        internal ErrorCodeException(ErrorCode errorCode)
            : base(errorCode.ToString())
        {
            ErrorCode = errorCode;
        }
    }

    internal static class CheckErrorCode
    {
        public static void Check(this CXErrorCode errorCode)
        {
            if (errorCode != CXErrorCode.CXError_Success)
            {
                throw new ErrorCodeException((ErrorCode)errorCode);
            }
        }
    }
}
