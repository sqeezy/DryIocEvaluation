using DryIoc;


var container = new Container(rules => rules
    .WithConcreteTypeDynamicRegistrations((serviceType, serviceKey) => true, Reuse.Singleton));

// container.Register<IMiddleInterface, MyService>(Reuse.Singleton);

container.RegisterMany<MyService>(Reuse.Singleton, serviceTypeCondition: type => type.IsInterface);

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

public class MyService : IMostSpecificInterface
{
    public bool IsDisposed { get; private set; }
    public void Dispose() => IsDisposed = true;
}