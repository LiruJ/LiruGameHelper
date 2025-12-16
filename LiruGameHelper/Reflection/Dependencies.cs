using System;
using System.Reflection;

namespace LiruGameHelper.Reflection;

public static class Dependencies
{
    public static object[] ResolveConstructorParameters(ConstructorInfo constructor, IServiceProvider? serviceProvider, object[]? inputs = null)
    {
        // Get the parameters.
        ParameterInfo[] parameters = constructor.GetParameters();

        // Create a new array to hold the objects.
        object[] dependencies = new object[parameters.Length];

        // Go over each parameter and find the service it's related to.
        for (int i = 0; i < parameters.Length; i++)
        {
            object? dependency = serviceProvider?.GetService(parameters[i].ParameterType);

            // If the parameter is not supported and cannot be supplied by the service provider, try get the input instead.
            if (dependency == null)
            {
                // If inputs were given, go over each one until the valid type is found.
                if (inputs != null)
                    foreach (object input in inputs)
                        if (parameters[i].ParameterType.IsAssignableFrom(input.GetType()))
                        {
                            dependency = input;
                            break;
                        }
            }

            // Add the parameter to the objects array.
            dependencies[i] = dependency ?? throw new Exception($"Cannot resolve dependencies, missing {parameters[i].ParameterType} {parameters[i].Name}.");
        }

        // Return the dependencies.
        return dependencies;
    }

    public static ConstructorInfo GetOnlyConstructor(this object input)
    {
        if (input is Type type)
            return GetOnlyConstructor(type);
        else
            throw new ArgumentException("Given argument was not a type!", nameof(input));
    }

    public static ConstructorInfo GetOnlyConstructor<T>() => GetOnlyConstructor(typeof(T));

    public static ConstructorInfo GetOnlyConstructor(Type type, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
    {
        // Ensure the given type is not null.
        ArgumentNullException.ThrowIfNull(type);

        // Get the constructors of the type.
        ConstructorInfo[] constructorInfos = type.GetConstructors(bindingFlags);

        // Ensure there's only one constructor.
        if (constructorInfos == null || constructorInfos.Length != 1)
            throw new Exception($"{type.Name} must have a single constructor that matches the given bindingFlags.");

        // Return the constructor.
        return constructorInfos[0];
    }

    public static T CreateObjectWithDependencies<T>(ConstructorInfo constructor, IServiceProvider serviceProvider, object[]? inputs = null)
        => (T)CreateObjectWithDependencies(constructor, serviceProvider, inputs);

    public static T CreateObjectWithDependencies<T>(IServiceProvider serviceProvider, object[]? inputs = null)
        => CreateObjectWithDependencies<T>(GetOnlyConstructor<T>(), serviceProvider, inputs);

    public static object CreateObjectWithDependencies(Type type, IServiceProvider serviceProvider, object[]? inputs = null)
    {
        // Get the constructor.
        ConstructorInfo constructor = GetOnlyConstructor(type);

        // Create and return the object.
        return CreateObjectWithDependencies(constructor, serviceProvider, inputs);
    }

    public static object CreateObjectWithDependencies(ConstructorInfo constructor, IServiceProvider? serviceProvider = null, object[]? inputs = null)
    {
        // Get the dependencies.
        object[] parameters = ResolveConstructorParameters(constructor, serviceProvider, inputs);

        // Create and return the object.
#if DEBUG
        try
        {
            return constructor.Invoke(parameters);
        }
        catch (TargetInvocationException targetException)
        {
            if (targetException.InnerException != null)
                throw targetException.InnerException;
            else
                throw;
        }
        catch (Exception)
        {
            throw;
        }
#elif RELEASE
        return constructor.Invoke(parameters);
#endif
    }
}