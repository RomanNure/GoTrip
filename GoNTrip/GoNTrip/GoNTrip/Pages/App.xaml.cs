using System;

using Autofac;

using Xamarin.Forms;

using GoNTrip.Controllers;
using GoNTrip.ServerAccess;
using GoNTrip.InternalDataAccess;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;

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

            builder.RegisterType<SignUpController>().SingleInstance().AsSelf();
            builder.RegisterType<SignUpQueryFactory>().SingleInstance().AsSelf();

            builder.RegisterType<LogInController>().SingleInstance().AsSelf();
            builder.RegisterType<LogInQueryFactory>().SingleInstance().AsSelf();

            builder.RegisterType<GetProfileController>().SingleInstance().AsSelf();
            builder.RegisterType<GetProfileQueryFactory>().SingleInstance().AsSelf();

            builder.RegisterType<ChangeAvatarController>().SingleInstance().AsSelf();
            builder.RegisterType<ChangeAvatarQueryFactory>().SingleInstance().AsSelf();

            builder.RegisterType<JsonResponseParser>().SingleInstance().As<IResponseParser>();

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
