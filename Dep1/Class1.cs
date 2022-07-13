using System.ComponentModel.Composition;

namespace Dep1;

public interface IDepService
{

}

[Export]
public class DepService : IDepService
{
    
}
