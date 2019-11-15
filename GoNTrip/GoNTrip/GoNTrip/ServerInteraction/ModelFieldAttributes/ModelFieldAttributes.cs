using System;

namespace GoNTrip.ServerInteraction.ModelFieldAttributes
{
    public abstract class QueryField : Attribute
    {
        public string Name { get; private set; }
        public QueryField(string name = null) => Name = name;
    }

    public class SignUpField : QueryField { public SignUpField(string name = null) : base(name) { } }
    public class LogInField : QueryField { public LogInField(string name = null) : base(name) { } }
    public class GetProfileField : QueryField { public GetProfileField(string name = null) : base(name) { } }
    public class UpdateProfileField : QueryField { public UpdateProfileField(string name = null) : base(name) { } }
    public class GetAdministratedCompaniesField : QueryField { public GetAdministratedCompaniesField(string name = null) : base(name) { } }
    public class GetUserByAdminField : QueryField { public GetUserByAdminField(string name = null) : base(name) { } }
    public class GetCompanyByAdminField : QueryField { public GetCompanyByAdminField(string name = null) : base(name) { } }
    public class GetOwnedCompaniesField : QueryField { public GetOwnedCompaniesField(string name = null) : base(name) { } }
    public class JoinTourField : QueryField { public JoinTourField(string name = null) : base(name) { } }
}
