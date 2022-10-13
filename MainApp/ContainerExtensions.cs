using System.ComponentModel.Composition;
using System.Reflection;
using DryIoc;

namespace MainApp;

public static class ContainerExtensions
{
    public static void RegisterSingletonsFromAssemblies<T>(this IContainer container)
    {
        Type type = typeof(T);
        Assembly assembly = type.GetAssembly();
        var types = assembly.GetTypes()
                            .Where(t => t.CustomAttributes.Select(l => l.AttributeType)
                                         .Contains(typeof(ExportAttribute)))
                            .ToArray();

        container.RegisterMany(types);
    }
}
