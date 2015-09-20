
using MTG_Scanner.Models;
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
            Bind<IWebcamController>().To<WebcamController>();
        }
    }
}
