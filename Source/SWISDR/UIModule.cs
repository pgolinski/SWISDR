using Autofac;
using SWISDR.Services;
using System.Windows;

namespace SWISDR
{
    public class UIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(ThisAssembly).AssignableTo<Window>();
            builder.RegisterType<ApplicationStateService>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
