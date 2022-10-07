using System.ComponentModel.Composition;
using Dep1;
using DryIoc;
using DryIoc.MefAttributedModel;
using DryIocAttributes;
using static System.Console;

void ServiceHierarchyTest(Container c)
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

var container = new Container(rules => rules.WithDefaultReuse(Reuse.Singleton));
// var container = new Container().WithMefAttributedModel(); 

// container.RegisterMany<MyService>(Reuse.Singleton, serviceTypeCondition: type => type.IsInterface);

container.RegisterExports(typeof(MyService).GetAssembly());

//ServiceHierarchyTest(container);

var thing = container.Resolve<IOperation<bool, bool>>();
var thing2 = container.Resolve<IOperation<object, string>>();
WriteLine(thing.GetType());
WriteLine(thing2.GetType());
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