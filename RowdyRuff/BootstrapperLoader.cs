using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RowdyRuff
{
    /// <summary>
    /// 'Inspired' by ASP.NET Core Hosting StartupLoader class at https://github.com/aspnet/Hosting
    /// </summary>
    public class BootstrapperLoader
    {
        private const string BootstrapperClassName = "Bootstrapper";
        private IEnumerable<object> _bootstrappers;
        private readonly string _dllPath;
        private readonly IHostingEnvironment _env;

        public BootstrapperLoader(string dllPath, IHostingEnvironment env)
        {
            _dllPath = dllPath;
            _env = env;
        }

        public void Initialize(string dllSearchPattern, IConfigurationRoot configurationRoot)
        {
            var dllNames = Directory.GetFiles(_dllPath, dllSearchPattern);
            var assemblies = dllNames.Select(TryLoadAssembly).Where(a => a != null);

            var assembliesWithBootstrapper =
                assemblies.Where(a => a.GetTypes().FirstOrDefault(t => t.Name == BootstrapperClassName) != null);

            _bootstrappers = assembliesWithBootstrapper.SelectMany(a => a.GetTypes()
                .Where(t => t.Name == BootstrapperClassName)
                .Select(bootstrapperType => CreateInstance(bootstrapperType, configurationRoot)));
        }        

        public void ConfigureServices(IServiceCollection services)
        {
            foreach (var bootstrapper in _bootstrappers)
            {
                bootstrapper.GetType()
                    .GetMethod("ConfigureServices", new Type[] { typeof(IServiceCollection) })?
                    .Invoke(bootstrapper, new object[] { services });
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            foreach (var bootstrapper in _bootstrappers)
            {
                InvokeMethodIfAvailable(bootstrapper, "Configure", app);

                if (_env.IsDevelopment())
                {
                    InvokeMethodIfAvailable(bootstrapper, "ConfigureDevelopment", app);
                }
            }
        }

        private static Assembly TryLoadAssembly(string path)
        {
            try
            {
                return Assembly.LoadFile(path);
            }
            catch (FileLoadException loadEx)
            { } // The Assembly has already been loaded.
            catch (BadImageFormatException imgEx)
            { } // If a BadImageFormatException exception is thrown, the file is not an assembly.

            return null;
        }

        private static void InvokeMethodIfAvailable(object bootstrapper, string methodName, IApplicationBuilder app)
        {
            var configureMethod = bootstrapper.GetType().GetMethod(methodName);
            if (configureMethod != null)
            {
                InvokeMethodWithDynamicallyResolvedParameters(bootstrapper, configureMethod, app);
            }
        }

        private static void InvokeMethodWithDynamicallyResolvedParameters(object bootstrapper, MethodInfo configureMethod, IApplicationBuilder builder)
        {
            var serviceProvider = builder.ApplicationServices;
            var parameterInfos = configureMethod.GetParameters();
            var parameters = new object[parameterInfos.Length];
            for (var index = 0; index < parameterInfos.Length; index++)
            {
                var parameterInfo = parameterInfos[index];
                if (parameterInfo.ParameterType == typeof(IApplicationBuilder))
                {
                    parameters[index] = builder;
                }
                else
                {
                    try
                    {
                        parameters[index] = serviceProvider.GetRequiredService(parameterInfo.ParameterType);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(
                            $"Could not resolve a service of type '{parameterInfo.ParameterType.FullName}' for the parameter '{parameterInfo.Name}' of method '{configureMethod.Name}' on type '{configureMethod.DeclaringType.FullName}'.",
                            ex);
                    }
                }
            }

            configureMethod.Invoke(bootstrapper, parameters);
        }

        private static object CreateInstance(Type instanceType, IConfigurationRoot configurationRoot)
        {
            var constructors = instanceType
                .GetTypeInfo()
                .DeclaredConstructors
                .Where(c => !c.IsStatic && c.IsPublic);

            var constructorWithConfigurationParameter = constructors.FirstOrDefault(c =>
            {
                var parameters = c.GetParameters();
                return parameters.Length == 1 && parameters.First().ParameterType == typeof(IConfigurationRoot);
            });

            return constructorWithConfigurationParameter != null ?
                constructorWithConfigurationParameter.Invoke(new object[] { configurationRoot }) :
                Activator.CreateInstance(instanceType);
        }
    }
}
