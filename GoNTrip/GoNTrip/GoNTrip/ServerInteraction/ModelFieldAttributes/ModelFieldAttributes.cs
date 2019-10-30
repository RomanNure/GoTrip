using System;

namespace GoNTrip.ServerInteraction.ModelFieldAttributes
{
    public class QueryField : Attribute { }

    public class SignUpField : QueryField { }
    public class LogInField : QueryField { }
    public class GetProfileField : QueryField { }
    public class UpdateProfileField : QueryField { }
    public class GetAdministratedCompaniesField : QueryField { }
    public class GetUserByAdminField : QueryField { }
    public class GetCompanyByAdminField : QueryField { }
    public class GetOwnedCompaniesField : QueryField { }
}
