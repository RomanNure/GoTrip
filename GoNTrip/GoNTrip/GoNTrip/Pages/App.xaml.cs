using System.Net;

using Autofac;

using Xamarin.Forms;

using GoNTrip.Util;
using GoNTrip.Model;
using GoNTrip.Controllers;
using GoNTrip.ServerAccess;
using GoNTrip.InternalDataAccess;
using GoNTrip.Pages.Additional.Controls;
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

            builder.RegisterType<Camera>().SingleInstance().AsSelf();
            builder.RegisterType<Gallery>().SingleInstance().AsSelf();
            builder.RegisterType<QrService>().SingleInstance().AsSelf();

            builder.RegisterType<ServerCommunicator>().SingleInstance().As<IServerCommunicator>();
            builder.RegisterType<JsonResponseParser>().SingleInstance().As<IResponseParser>();
            builder.RegisterType<CookieContainer>().SingleInstance().AsSelf();

            builder.RegisterType<SignUpController>().SingleInstance().AsSelf();
            builder.RegisterType<SignUpQueryFactory>().SingleInstance().AsSelf();

            builder.RegisterType<LogInController>().SingleInstance().AsSelf();
            builder.RegisterType<LogInQueryFactory>().SingleInstance().AsSelf();

            builder.RegisterType<GetProfileController>().SingleInstance().AsSelf();
            builder.RegisterType<GetProfileQueryFactory>().SingleInstance().AsSelf();

            builder.RegisterType<ChangeAvatarController>().SingleInstance().AsSelf();
            builder.RegisterType<ChangeAvatarQueryFactory>().SingleInstance().AsSelf();

            builder.RegisterType<UpdateProfileController>().SingleInstance().AsSelf();
            builder.RegisterType<UpdateProfileQueryFactory>().SingleInstance().AsSelf();

            builder.RegisterType<GetOwnedCompaniesController>().SingleInstance().AsSelf();
            builder.RegisterType<GetOwnedCompaniesQueryFactory>().SingleInstance().AsSelf();

            builder.RegisterType<GetAdministratedCompaniesController>().SingleInstance().AsSelf();
            builder.RegisterType<GetAdministratedCompaniesQueryFactory>().SingleInstance().AsSelf();

            builder.RegisterType<GetToursController>().SingleInstance().AsSelf();
            builder.RegisterType<GetToursQueryFactory>().SingleInstance().AsSelf();

            builder.RegisterType<GetUserByAdminController>().SingleInstance().AsSelf();
            builder.RegisterType<GetUserByAdminQueryFactory>().SingleInstance().AsSelf();

            builder.RegisterType<GetCompanyByAdminController>().SingleInstance().AsSelf();
            builder.RegisterType<GetCompanyByAdminQueryFactory>().SingleInstance().AsSelf();

            builder.RegisterType<JoinTourController>().SingleInstance().AsSelf();
            builder.RegisterType<JoinTourQueryFactory>().SingleInstance().AsSelf();

            builder.RegisterType<TourListItemFactory>().SingleInstance().AsSelf();
            builder.RegisterType<Session>().SingleInstance().AsSelf();

            DI = builder.Build();
        }

        public App()
        {
            InitServices();
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
