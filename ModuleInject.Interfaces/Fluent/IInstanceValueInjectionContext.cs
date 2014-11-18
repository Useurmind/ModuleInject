namespace ModuleInject.Interfaces.Fluent
{
    public interface IInstanceValueInjectionContext<IComponent, TComponent, IModule, TModule, TValue>
        where TComponent : IComponent
        where TModule : IModule
        where IModule : IInjectionModule
    {
        
    }
}