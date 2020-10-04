// source: https://github.com/dotnet/runtime/blob/72d1f6bbb4aa39d71e2348ed9621129c63a7c04a/src/libraries/Microsoft.Extensions.Configuration.FileExtensions/src/FileLoadExceptionContext.cs
// Original Copyright Notice:
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Grillisoft.Configuration
{
    /// <summary>
    /// Contains information about a file load exception.
    /// </summary>
    public class FileKeyLoadExceptionContext
    {
        /// <summary>
        /// The <see cref="FileConfigurationProvider"/> that caused the exception.
        /// </summary>
        public FileKeyConfigurationProvider Provider { get; set; }

        /// <summary>
        /// The exception that occurred in Load.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// If true, the exception will not be rethrown.
        /// </summary>
        public bool Ignore { get; set; }
    }
}