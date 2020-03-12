using AppService.Providers.Implementation;
using AppService.Providers.Interfaces;
using AppService.Providers.mplementation;

namespace AppService
{
    public static class Module
    {
        public static void Init()
        {
            Repository.Module.Init();
            IoC.Manager.Register<IUserProvider, UserProvider>();
            IoC.Manager.Register<IPopulationProvider,PopulationProvider>();
            
        }
    }
}