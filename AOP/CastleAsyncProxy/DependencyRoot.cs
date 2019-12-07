using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CastleAsyncProxy.Logic;

namespace CastleAsyncProxy
{
    internal static class DependencyRoot
    {
        private static readonly IWindsorContainer Container;

        static DependencyRoot()
        {
            Container = new WindsorContainer();
            RegisterDependencies(Container);
        }

        public static void Dispose() => Container.Dispose();

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        private static void RegisterDependencies(IWindsorContainer container)
        {
            container.Register(Component.For<AsyncInterceptor>().ImplementedBy<AsyncInterceptor>());
            container.Register(Component.For<IRepository>().ImplementedBy<Repository>().Interceptors<AsyncInterceptor>());
            container.Register(Component.For<IService>().ImplementedBy<Service>().Interceptors<AsyncInterceptor>());
        }
    }
}
