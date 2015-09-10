
using MTG_Scanner.Models;
using Ninject.Extensions.Factory;
using Ninject.Modules;

namespace MTG_Scanner.Utils.Impl
{
    class BindHouse : NinjectModule
    {
        public override void Load()
        {
            Bind<IMagicCardFactory>().ToFactory();
            Bind<IUtil>().To<Util>().InSingletonScope();
        }
    }
}
