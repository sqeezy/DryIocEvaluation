using System.ComponentModel.Composition;
using System.Diagnostics;
using Dep1;
using DryIoc;
using MainApp;
using static System.Console;

void LoadFromLibTest(IContainer c)
{
    var abstractService = c.Resolve<IDepService>();
    Debug.Assert(abstractService.GetType() == typeof(DepService));

    var specificService = c.Resolve<DepService>();
    Debug.Assert(specificService.GetType() == typeof(DepService));
}

void OpenGenericsTest(IContainer c)
{
    var thing = c.Resolve<IOperation<bool, bool>>();
    var thing2 = c.Resolve<IOperation<object, string>>();
    WriteLine(thing.GetType());
    WriteLine(thing2.GetType());
}

void ServiceHierarchyTest(IContainer c)
{
    // var service = c.Resolve<MyService>();
    // var serviceAsInterface = c.Resolve<IMyService>();
    // var serviceAsInterfaceToTheSide = c.Resolve<IServiceToTheSideOfMyService>();
    var serviceAsMiddleInterface = c.Resolve<IMostSpecificInterface>();
    var serviceAsMiddleInterface2 = c.Resolve<IMostSpecificInterface>();

    c.Dispose();

    WriteLine($"{serviceAsMiddleInterface2 == serviceAsMiddleInterface}");
    WriteLine($"{serviceAsMiddleInterface.IsDisposed}");
    WriteLine($"{serviceAsMiddleInterface2.IsDisposed}");
}

var container = new Container(rules => rules.WithDefaultReuse(Reuse.Singleton));

container.RegisterSingletonsFromAssemblies<MyService>();
container.RegisterSingletonsFromAssemblies<DepService>();

LoadFromLibTest(container);
OpenGenericsTest(container);

ServiceHierarchyTest(container);

ReadKey();

public interface IOperation<TSubject, TScope> { }

[Export]
public class OperationClosed : IOperation<bool, bool> { }

[Export]
public class OperationOpen<TSubject, TScope> : IOperation<TSubject, TScope> { }

public interface IServiceToTheSideOfMyService { }

public interface IMyService : IDisposable
{
    bool IsDisposed { get; }
}

public interface IMostSpecificInterface : IMyService, IServiceToTheSideOfMyService { }

[Export]
public class MyService : IMostSpecificInterface
{
    public bool IsDisposed { get; private set; }
    public void Dispose() => IsDisposed = true;
}
