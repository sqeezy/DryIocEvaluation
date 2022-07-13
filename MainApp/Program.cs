using System.ComponentModel.Composition;
using Dep1;
using DryIoc;
using DryIoc.MefAttributedModel;


var container = new Container(rules => rules
    .WithConcreteTypeDynamicRegistrations((serviceType, serviceKey) => true, Reuse.Singleton)).WithMef();

// container.RegisterMany<MyService>(Reuse.Singleton, serviceTypeCondition: type => type.IsInterface);

container.RegisterExports(new []{typeof(MyService)});

// var service = container.Resolve<MyService>();
var serviceAsInterface = container.Resolve<IMyService>();
var serviceAsInterfaceToTheSide = container.Resolve<IServiceToTheSideOfMyService>();
var serviceAsMiddleInterface = container.Resolve<IMostSpecificInterface>();

container.Dispose();

Console.WriteLine($"{serviceAsInterface == serviceAsMiddleInterface}");
Console.WriteLine($"{serviceAsInterface.IsDisposed}");




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

[Export(typeof(IMyService))]
public class MyService : IMostSpecificInterface
{
    public bool IsDisposed { get; private set; }
    public void Dispose() => IsDisposed = true;
}