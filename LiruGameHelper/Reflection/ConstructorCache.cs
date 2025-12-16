using System;
using System.Collections.Generic;
using System.Reflection;

namespace LiruGameHelper.Reflection;

public class ConstructorCache<T> where T : notnull
{
    #region Fields
    private readonly Dictionary<Type, ConstructorInfo> constructorsByType = [];

    private readonly Dictionary<string, Type> typesByName = [];
    #endregion

    #region Constructors
    public ConstructorCache(Assembly assembly, string? defaultPath = null)
    {
        // Load the default namespace.
        if (!string.IsNullOrWhiteSpace(defaultPath))
            RegisterNamespace(assembly, defaultPath);
    }

    public ConstructorCache() { }
    #endregion

    #region Registration Functions
    /// <summary> Registers all the <see cref="Type"/>s in the given <paramref name="namespacePath"/> in the given <paramref name="assembly"/>. </summary>
    /// <param name="assembly"> The <see cref="Assembly"/> in which the <see cref="Type"/>s are defined. </param>
    /// <param name="namespacePath"> The namespace of the <see cref="Type"/>s. </param>
    public void RegisterNamespace(Assembly assembly, string namespacePath)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        if (string.IsNullOrWhiteSpace(namespacePath))
            throw new ArgumentException("Element namespace cannot be null or empty.", nameof(namespacePath));

        foreach (Type type in assembly.GetTypes())
        {
            // If the type has no namespace, skip it.
            if (string.IsNullOrWhiteSpace(type.Namespace))
                continue;

            // Otherwise, check to see if it is valid.
            if (type.Namespace.Contains(namespacePath))
                RegisterType(type);
        }
    }

    public bool RegisterType(Type type)
    {
        // Check that the type is valid.
        if (!type.IsClass || type.IsAbstract || !typeof(T).IsAssignableFrom(type))
            return false;

        // Get the constructor for the type.
        ConstructorInfo componentConstructor = Dependencies.GetOnlyConstructor(type);

        // Register the type.
        typesByName.Add(type.Name, type);

        // Add the first constructor to the dictionary.
        constructorsByType.Add(type, componentConstructor);

        // Return true since the addition was successful.
        return true;
    }
    #endregion

    #region Get Functions
    public ConstructorInfo GetConstructor(string name) => GetConstructor(GetTypeFromName(name));

    public ConstructorInfo GetConstructor(Type type)
    {
        // Get the constructor of the type from the name, throw an exception if it is not registered.
        if (!constructorsByType.TryGetValue(type, out ConstructorInfo? constructor))
            throw new Exception($"The type with the name \"{type.Name}\" has no constructor registered in this cache.");

        // return the constructor.
        return constructor;
    }

    public Type GetTypeFromName(string name)
    {
        // Get the type from the name, throw an exception if it is not registered.
        if (!typesByName.TryGetValue(name, out Type? componentType))
            throw new Exception($"No component with the name \"{name}\" is registered.");

        // return the type.
        return componentType;
    }

    public bool TryGetTypeFromName(string name, out Type? type)
        => typesByName.TryGetValue(name, out type);

    public bool HasType<T1>() => constructorsByType.ContainsKey(typeof(T1));

    public bool HasType(Type type) => constructorsByType.ContainsKey(type);

    public T CreateInstance(string name, IServiceProvider? serviceProvider = null, object[]? inputs = null)
        => (T)Dependencies.CreateObjectWithDependencies(GetConstructor(name), serviceProvider, inputs);

    public T1 CreateInstance<T1>(string name, IServiceProvider? serviceProvider = null, object[]? inputs = null) where T1 : T
        => (T1)CreateInstance(name, serviceProvider, inputs);

    public T CreateInstance(string name, params object[] parameters)
        => (T)GetConstructor(name).Invoke(parameters);

    public T1 CreateInstance<T1>(string name, params object[] parameters) where T1 : T
        => (T1)CreateInstance(name, parameters);
    #endregion
}
