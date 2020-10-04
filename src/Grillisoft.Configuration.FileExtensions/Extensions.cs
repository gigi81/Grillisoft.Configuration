using System;
using Microsoft.Extensions.Configuration;

namespace Grillisoft.Configuration
{
    public static class FileKeyConfigurationExtensions
    {
        private static string FileLoadExceptionHandlerKey = "FileLoadExceptionHandler";

        /// <summary>
        /// Gets the default <see cref="IFileProvider"/> to be used for file-based providers.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static Action<FileTreeLoadExceptionContext> GetFileKeyLoadExceptionHandler(this IConfigurationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (builder.Properties.TryGetValue(FileLoadExceptionHandlerKey, out object handler))
            {
                return handler as Action<FileTreeLoadExceptionContext>;
            }
            return null;
        }
    }
}
