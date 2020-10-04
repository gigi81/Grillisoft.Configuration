// source: https://github.com/dotnet/runtime/blob/master/src/libraries/Microsoft.Extensions.Configuration.FileExtensions/src/FileConfigurationSource.cs
// Original Copyright Notice:
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Grillisoft.Configuration
{
    public abstract class FileTreeConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// The path to the directory containging the configuation files.
        /// </summary>
        public string DirectoryPath { get; set; }

        /// <summary>
        /// Extensions of the configuration files to load
        /// </summary>
        public abstract string Extension { get; }

        /// <summary>
        /// Keys used to find the configuration to load
        /// </summary>
        public virtual IEnumerable<string> Keys { get; set; }

        /// <summary>
        /// Used to access the contents of the file.
        /// </summary>
        public IFileProvider FileProvider { get; set; }

        /// <summary>
        /// Determines if loading the file is optional.
        /// </summary>
        public bool Optional { get; set; }

        /// <summary>
        /// Determines whether the source will be loaded if the underlying file changes.
        /// </summary>
        public bool ReloadOnChange { get; set; }

        /// <summary>
        /// Number of milliseconds that reload will wait before calling Load.  This helps
        /// avoid triggering reload before a file is completely written. Default is 250.
        /// </summary>
        public int ReloadDelay { get; set; } = 250;

        /// <summary>
        /// Will be called if an uncaught exception occurs in FileConfigurationProvider.Load.
        /// </summary>
        public Action<FileTreeLoadExceptionContext> OnLoadException { get; set; }

        /// <summary>
        /// Builds the <see cref="IConfigurationProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>A <see cref="IConfigurationProvider"/></returns>
        public abstract IConfigurationProvider Build(IConfigurationBuilder builder);

        /// <summary>
        /// Called to use any default settings on the builder like the FileProvider or FileLoadExceptionHandler.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        public void EnsureDefaults(IConfigurationBuilder builder)
        {
            FileProvider = FileProvider ?? builder.GetFileProvider();
            OnLoadException = OnLoadException ?? builder.GetFileKeyLoadExceptionHandler();
        }

        /// <summary>
        /// If no file provider has been set, for absolute Path, this will create a physical file provider 
        /// for the nearest existing directory.
        /// </summary>
        public void ResolveFileProvider()
        {
            if (FileProvider != null || string.IsNullOrEmpty(DirectoryPath))
                return;

            var directory = Path.GetFullPath(DirectoryPath);
            if (Directory.Exists(directory))
                FileProvider = new PhysicalFileProvider(directory);
        }
    }
}
