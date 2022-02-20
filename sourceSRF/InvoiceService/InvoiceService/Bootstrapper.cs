using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using FX.Context;
using FX.Core;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceService
{
    public class Bootstrapper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Bootstrapper));
        private static IWindsorContainer container;
        public static void InitializeContainer()
        {
            try
            {
                // Initialize Windsor
                container = new WindsorContainer(new XmlInterpreter());

                //container = new WindsorContainer(new XmlInterpreter(new ConfigResource("castle")));

                // Inititialize the static Windsor helper class. 
                IoC.Initialize(container);

                // Add ICuyahogaContext to the container.
                container.Register(Component.For<IFXContext>()
                    .ImplementedBy<FXContext>()
                    .Named("FX.context")
                    .LifeStyle.PerWebRequest
                );
            }
            catch (Exception ex)
            {
                log.Error("Error initializing application.", ex);
                throw;
            }
        }
    }
}
