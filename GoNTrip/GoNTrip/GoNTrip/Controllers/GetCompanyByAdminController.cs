using Autofac;
using GoNTrip.Model;
using GoNTrip.Pages;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoNTrip.Controllers
{
    public class GetCompanyByAdminController
    {
        public async Task<Company> GetCompanyByAdmin(Admin admin)
        {
            IQuery getCompanyByAdminQuery = await App.DI.Resolve<GetCompanyByAdminQueryFactory>().GetCompanyByAdmin(admin);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(getCompanyByAdminQuery);
            return App.DI.Resolve<IResponseParser>().Parse<Company>(response);
        }
    }
}
