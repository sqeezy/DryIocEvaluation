using DryIocAttributes;

namespace Dep1;

public interface IDepService { }

[ExportMany]
public class DepService : IDepService { }
