using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Reflection;
using Dep1;
using DryIoc;
using DryIoc.MefAttributedModel;
using DryIocAttributes;
using static System.Console;

void LoadFromLibTest(IContainer c)
{
    Assembly assembly = typeof(DepService).GetAssembly();

    var type = assembly.GetType(nameof(DepService));
    var attributes = type.Attributes;
    
    Assembly[] assemblies = { assembly };
    var findings = AttributedModel.Scan(assemblies);
    
    c.RegisterExports(assemblies);
    var depsService = c.Resolve<IDepService>();
    Debug.Assert(depsService.GetType() == typeof(DepService));
}

void OpenGenericsTest(IContainer c)
{
    c.RegisterExports(typeof(MyService).Assembly);
    var thing = c.Resolve<IOperation<bool, bool>>();
    var thing2 = c.Resolve<IOperation<object, string>>();
    WriteLine(thing.GetType());
    WriteLine(thing2.GetType());
}

void ServiceHierarchyTest(IContainer c)
{
    // var service = container.Resolve<MyService>();
    // var serviceAsInterface = container.Resolve<IMyService>();
    // var serviceAsInterfaceToTheSide = container.Resolve<IServiceToTheSideOfMyService>();
    var serviceAsMiddleInterface = c.Resolve<IMostSpecificInterface>();
    var serviceAsMiddleInterface2 = c.Resolve<IMostSpecificInterface>();

    c.Dispose();

    WriteLine($"{serviceAsMiddleInterface2 == serviceAsMiddleInterface}");
    WriteLine($"{serviceAsMiddleInterface.IsDisposed}");
    WriteLine($"{serviceAsMiddleInterface2.IsDisposed}");
}

var container = new Container(rules => rules.WithDefaultReuse(Reuse.Singleton)).WithMefAttributedModel();

LoadFromLibTest(container);
//ServiceHierarchyTest(container);
// OpenGenericsTest(container);

ReadKey();

public interface IOperation<TSubject, TScope>{}

[ExportMany]
public class OperationClosed : IOperation<bool, bool>{}

[ExportMany]
public class OperationOpen<TSubject, TScope> : IOperation<TSubject, TScope>{}

public interface IServiceToTheSideOfMyService
{

}

public interface IMyService : IDisposable
{
    bool IsDisposed { get; }
}

public interface IMostSpecificInterface : IMyService, IServiceToTheSideOfMyService
{

}

// [Export(typeof(IMyService))]
// [Export(typeof(IMostSpecificInterface)), SingletonReuse]
[Export]
public class MyService : IMostSpecificInterface
{
    public bool IsDisposed { get; private set; }
    public void Dispose() => IsDisposed = true;
}