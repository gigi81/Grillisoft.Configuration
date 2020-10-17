using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Grillisoft.Configuration.Dolt;

namespace Grillisoft.Configuration
{
    public static class DoltConfigurationExtensions
    {
        public static IConfigurationBuilder AddDoltTree(this IConfigurationBuilder builder, string url, IEnumerable<string> keys)
        {
            return AddDoltTree(builder, url, null, keys);
        }

        public static IConfigurationBuilder AddDoltTree(this IConfigurationBuilder builder, string url, string branch, IEnumerable<string> keys)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            return builder.AddDoltTree(s =>
            {
                s.Url = url;
                s.Branch = branch;
                s.Keys = keys;
            });
        }

        public static IConfigurationBuilder AddDoltTree(this IConfigurationBuilder builder, Action<DoltConfigurationSource> configureSource)
            => builder.Add(configureSource);
    }
}
