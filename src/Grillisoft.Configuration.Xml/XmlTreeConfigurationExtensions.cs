using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Grillisoft.Configuration.Xml
{
    public static class XmlTreeConfigurationExtensions
    {
        /// <summary>
        /// Adds the XML configuration provider at <paramref name="path"/> to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="path">Path relative to the base path stored in 
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddXmlTree(this IConfigurationBuilder builder, IEnumerable<string> keys)
        {
            return AddXmlTree(builder, provider: null, directoryPath: ".", keys: keys, optional: false, reloadOnChange: false);
        }

        /// <summary>
        /// Adds the XML configuration provider at <paramref name="path"/> to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="path">Path relative to the base path stored in 
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddXmlTree(this IConfigurationBuilder builder, string directoryPath, IEnumerable<string> keys)
        {
            return AddXmlTree(builder, provider: null, directoryPath: directoryPath, keys: keys, optional: false, reloadOnChange: false);
        }

        /// <summary>
        /// Adds the XML configuration provider at <paramref name="path"/> to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="path">Path relative to the base path stored in 
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddXmlTree(this IConfigurationBuilder builder, string directoryPath, IEnumerable<string> keys, bool optional)
        {
            return AddXmlTree(builder, provider: null, directoryPath: directoryPath, keys: keys, optional: optional, reloadOnChange: false);
        }

        /// <summary>
        /// Adds the XML configuration provider at <paramref name="path"/> to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="path">Path relative to the base path stored in 
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddXmlTree(this IConfigurationBuilder builder, string directoryPath, IEnumerable<string> keys, bool optional, bool reloadOnChange)
        {
            return AddXmlTree(builder, provider: null, directoryPath: directoryPath, keys: keys, optional: optional, reloadOnChange: reloadOnChange);
        }

        /// <summary>
        /// Adds a XML configuration source to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="provider">The <see cref="IFileProvider"/> to use to access the file.</param>
        /// <param name="path">Path relative to the base path stored in 
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddXmlTree(this IConfigurationBuilder builder, IFileProvider provider, string directoryPath, IEnumerable<string> keys, bool optional, bool reloadOnChange)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (string.IsNullOrEmpty(directoryPath))
                throw new ArgumentException(Resources.Error_InvalidDirectoryPath, nameof(directoryPath));

            return builder.AddXmlTree(s =>
            {
                s.FileProvider = provider;
                s.DirectoryPath = directoryPath;
                s.Keys = keys;
                s.Optional = optional;
                s.ReloadOnChange = reloadOnChange;
                s.ResolveFileProvider();
            });
        }

        /// <summary>
        /// Adds a XML configuration source to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="configureSource">Configures the source.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddXmlTree(this IConfigurationBuilder builder, Action<XmlTreeConfigurationSource> configureSource)
            => builder.Add(configureSource);
    }
}
