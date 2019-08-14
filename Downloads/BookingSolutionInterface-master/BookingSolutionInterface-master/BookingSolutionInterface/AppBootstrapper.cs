using BookingSolutionInterface.ViewModels;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Windows;
using BookingSolutionInterface.Utilities;

namespace BookingSolutionInterface
{
    public class AppBootstrapper : BootstrapperBase
    {
        private SimpleContainer container;

        public AppBootstrapper()
        {
            Initialize();

            //dataSetup _config = new dataSetup();

            //_config.createXMLDBFolder();

        }

        protected override void Configure()
        {
            container = new SimpleContainer();
            container.Instance<SimpleContainer>(container);

            //ViewModels
            container.Singleton<IndexViewModel>();
            container.PerRequest<Ver1ServiceFeeViewModel>();
            container.PerRequest<AmadeusExtractorViewModel>();
            container.PerRequest<SabreExtractorViewModel>();
            container.PerRequest<CebuPacExtractorViewModel>();
            container.PerRequest<ServiceFeeViewModel>();
            container.PerRequest<ReqeustForInvoiceViewModel>();
            container.PerRequest<PNRReportViewModel>();
            container.PerRequest<FareQuoteViewModel>();


            //Services
            container.PerRequest<IWindowManager, MyWindowManager>();
            //container.PerRequest<ISettingsService, MySettingsService>();
            container.Singleton<Interface.INavigationService, NavigationService>();
            

            //AutoMapper
            //Mapper.CreateMap<Client, Client>();
            //Mapper.CreateMap<BookCategory, BookCategory>();
            //Mapper.CreateMap<Book, Book>();
            //Mapper.CreateMap<Employee, Employee>();
            //Mapper.CreateMap<Lending, Lending>();
            //Mapper.CreateMap<Address, Address>();
            //Mapper.CreateMap<LentBook, LentBook>();
            //Mapper.CreateMap<Person, Person>();
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            return container.GetInstance(serviceType, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return container.GetAllInstances(serviceType);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            Interface.INavigationService nav = (Interface.INavigationService)container.GetInstance(typeof(Interface.INavigationService), null);
            

            //nav.GetWindow<IndexViewModel>().ShowWindow();

            bool result = nav.GetWindow<IndexViewModel>()
                             .ShowWindowModal(new Dictionary<string, object>() { { "WindowStyle", WindowStyle.ToolWindow }, { "ResizeMode", ResizeMode.NoResize } });

            if (!result)
                Application.Current.Shutdown();
            else
                ((IndexViewModel)container.GetInstance(typeof(IndexViewModel), null)).RefreshTransactions();

        }


    }
}
