﻿// source: https://github.com/dotnet/runtime/blob/master/src/libraries/Microsoft.Extensions.Configuration.FileExtensions/src/FileConfigurationProvider.cs
// Original Copyright Notice:
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Grillisoft.Configuration
{
    /// <summary>
    /// Base class for file based <see cref="ConfigurationProvider"/>.
    /// </summary>
    public abstract class FileTreeConfigurationProvider<TConfigurationSource> : ConfigurationProvider, IDisposable where TConfigurationSource : FileTreeConfigurationSource
    {
        private readonly IDisposable _changeTokenRegistration;

        /// <summary>
        /// Initializes a new instance with the specified source.
        /// </summary>
        /// <param name="source">The source settings.</param>
        public FileTreeConfigurationProvider(TConfigurationSource source)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));

            if (Source.ReloadOnChange && Source.FileProvider != null)
            {
                _changeTokenRegistration = ChangeToken.OnChange(
                    () => Source.FileProvider.Watch("*." + Source.Extension),
                    () => {
                        Thread.Sleep(Source.ReloadDelay);
                        Load(reload: true);
                    });
            }
        }

        /// <summary>
        /// The source settings for this provider.
        /// </summary>
        public TConfigurationSource Source { get; }

        private void Load(bool reload)
        {
            IFileInfo file = null;
            
            foreach(var key in Source.Keys)
            {
                file = Source.FileProvider?.GetFileInfo(key + "." + Source.Extension);
                if (file.Exists)
                    break;
            }

            if (file == null || !file.Exists)
            {
                if (Source.Optional || reload) // Always optional on reload
                {
                    Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                }
                else
                {
                    var error = new StringBuilder($"The configuration directory '{Source.DirectoryPath}' was not found and is not optional.");
                    if (!string.IsNullOrEmpty(file?.PhysicalPath))
                    {
                        error.Append($" The physical path is '{file.PhysicalPath}'.");
                    }
                    throw new FileNotFoundException(error.ToString());
                }
            }
            else
            {
                // Always create new Data on reload to drop old keys
                if (reload)
                    Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                Data = LoadInternal(file, out var parentKey);

                while(!String.IsNullOrEmpty(parentKey))
                {
                    var parentFile = Source.FileProvider?.GetFileInfo(parentKey + "." + Source.Extension);
                    if (parentFile == null || !parentFile.Exists)
                        if (Source.Optional || reload) // Always optional on reload
                            break;

                    foreach (var pair in LoadInternal(parentFile, out parentKey))
                    {
                        if (!Data.ContainsKey(pair.Key))
                            Data.Add(pair);
                    }
                }
            }

            // REVIEW: Should we raise this in the base as well / instead?
            OnReload();
        }

        private IDictionary<string, string> LoadInternal(IFileInfo file, out string parent)
        {
            parent = null;

            static Stream OpenRead(IFileInfo fileInfo)
            {
                if (fileInfo.PhysicalPath != null)
                {
                    // The default physical file info assumes asynchronous IO which results in unnecessary overhead
                    // especally since the configuration system is synchronous. This uses the same settings
                    // and disables async IO.
                    return new FileStream(
                        fileInfo.PhysicalPath,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.ReadWrite,
                        bufferSize: 1,
                        FileOptions.SequentialScan);
                }

                return fileInfo.CreateReadStream();
            }

            using (var stream = OpenRead(file))
            {
                try
                {
                    return Load(stream, out parent);
                }
                catch (Exception e)
                {
                    HandleException(ExceptionDispatchInfo.Capture(e));
                }
            }

            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Loads the contents of the file at <see cref="Path"/>.
        /// </summary>
        /// <exception cref="FileNotFoundException">If Optional is <c>false</c> on the source and a
        /// file does not exist at specified Path.</exception>
        public override void Load()
        {
            Load(reload: false);
        }

        /// <summary>
        /// Loads this provider's data from a stream.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <param name="parent">Name of the parent to load</param>
        public abstract IDictionary<string, string> Load(Stream stream, out string parent);

        private void HandleException(ExceptionDispatchInfo info)
        {
            var ignoreException = false;

            if (Source.OnLoadException != null)
            {
                var exceptionContext = new FileTreeLoadExceptionContext
                {
                    Provider = this,
                    Exception = info.SourceException
                };
                Source.OnLoadException.Invoke(exceptionContext);
                ignoreException = exceptionContext.Ignore;
            }

            if (!ignoreException)
                info.Throw();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the provider.
        /// </summary>
        /// <param name="disposing"><c>true</c> if invoked from <see cref="IDisposable.Dispose"/>.</param>
        protected virtual void Dispose(bool disposing)
        {
            _changeTokenRegistration?.Dispose();
        }
    }
}
