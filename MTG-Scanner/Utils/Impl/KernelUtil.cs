using Ninject;

namespace MTG_Scanner.Utils.Impl
{
    static class KernelUtil
    {
        public static void CreateKernel()
        {
            Kernel = new StandardKernel();
            Kernel.Load(new BindHouse());
        }

        public static StandardKernel Kernel { get; set; }
    }
}
