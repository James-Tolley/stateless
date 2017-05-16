﻿using System;

namespace Stateless
{
    /// <summary>
    /// Describes a method - either an action (activate, deactivate, etc.) or a transition guard
    /// </summary>
    public class MethodDescription
    {
        readonly string _description;                     // _description can be null if user didn't specify a description

        /// <summary>
        /// Is the method synchronous or asynchronous?
        /// </summary>
        internal enum Timing
        {
            /// <summary>Method is synchronous</summary>
            Synchronous,

            /// <summary>Method is asynchronous</summary>
            Asynchronous
        }
        readonly Timing _timing;

        internal static MethodDescription Create(Delegate method, string description, Timing timing = Timing.Synchronous)
        {
            return new MethodDescription(method?.TryGetMethodName(), description, timing);
        }

        MethodDescription(string methodName, string description, Timing timing)      // description can be null if user didn't specify a description
        {
            MethodName = methodName;
            _description = description;
            _timing = timing;
        }

        /// <summary>
        /// The name of the invoked method.  If the method is a lambda or delegate, the name will be a compiler-generated
        /// name that is often not human-friendly (e.g. "(.ctor)b__2_0" except with angle brackets instead of parentheses)
        /// </summary>
        public string MethodName { get; private set; }

        /// <summary>
        /// Text returned for compiler-generated functions where the caller has not specified a description
        /// </summary>
        static public string DefaultFunctionDescription { get; set; } = "Function";

        /// <summary>
        /// A description of the invoked method.  Returns:
        /// 1) The user-specified description, if any
        /// 2) else if the method name is compiler-generated, returns DefaultFunctionDescription
        /// 3) else the method name
        /// </summary>
        public string Description
        {
            get
            {
                if (_description != null)
                    return _description;
                if (MethodName.IndexOfAny(new char[] { '<', '>' }) >= 0)
                    return DefaultFunctionDescription;
                return MethodName ?? "<null>";
            }
        }

        /// <summary>
        /// Returns true if the method is invoked asynchronously.
        /// </summary>
        public bool IsAsync => (_timing == Timing.Asynchronous);
    }
}
