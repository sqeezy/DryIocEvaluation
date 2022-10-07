using System.ComponentModel.Composition;
using Dep1;
using DryIoc;
using DryIoc.MefAttributedModel;
using DryIocAttributes;

// var container = new Container();
var container = new Container().WithMefAttributedModel(); 

// container.RegisterMany<MyService>(Reuse.Singleton, serviceTypeCondition: type => type.IsInterface);

container.RegisterExports(typeof(MyService).GetAssembly());

// var service = container.Resolve<MyService>();
// var serviceAsInterface = container.Resolve<IMyService>();
// var serviceAsInterfaceToTheSide = container.Resolve<IServiceToTheSideOfMyService>();
var serviceAsMiddleInterface = container.Resolve<IMostSpecificInterface>();
var serviceAsMiddleInterface2 = container.Resolve<IMostSpecificInterface>();

container.Dispose();

Console.WriteLine($"{serviceAsMiddleInterface2 == serviceAsMiddleInterface}");
Console.WriteLine($"{serviceAsMiddleInterface.IsDisposed}");
Console.WriteLine($"{serviceAsMiddleInterface2.IsDisposed}");




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