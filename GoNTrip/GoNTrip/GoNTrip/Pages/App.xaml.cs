using Autofac;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

using GoNTrip.ServerAccess;
using GoNTrip.InternalDataAccess;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;
using GoNTrip.ServerInteraction.ResponseParsers.Auth;

using System;

namespace GoNTrip.Pages
{
    public partial class App : Application
    {
        public static IContainer DI { get; private set; }

        public void InitServices()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<FileManager>().AsSelf();
            builder.RegisterType<JsonConfigurationManager>().As<IConfigManager>();

            builder.RegisterType<ServerCommunicator>().SingleInstance().As<IServerCommunicator>();

            builder.RegisterType<AuthQueryFactory>().SingleInstance().AsSelf();

            builder.RegisterType<SignUpResponseParser>().SingleInstance().AsSelf();
            builder.RegisterType<LogInResponseParser>().SingleInstance().AsSelf();

            DI = builder.Build();
        }

        public App()
        {
            TimeSpan start = DateTime.Now.TimeOfDay;
            InitServices();
            TimeSpan end = DateTime.Now.TimeOfDay;

            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
