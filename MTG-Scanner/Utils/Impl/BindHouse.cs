
using MTG_Scanner.Models;
using MTG_Scanner.Models.Impl;
using MTG_Scanner.VMs;
using Ninject.Modules;

namespace MTG_Scanner.Utils.Impl
{
    class BindHouse : NinjectModule
    {
        public override void Load()
        {
            //Bind<IMagicCardFactory>().ToFactory();
            Bind<IUtil>().To<Util>().InSingletonScope();
            Bind<IXmlFileCreator>().To<XmlFileCreator>();
            Bind<ICardDatabase>().To<CardDatabase>().InSingletonScope();
            Bind<IWebcamController>().To<WebcamController>();
            Bind<MainWindowViewModel>().ToSelf();
        }
    }
}
