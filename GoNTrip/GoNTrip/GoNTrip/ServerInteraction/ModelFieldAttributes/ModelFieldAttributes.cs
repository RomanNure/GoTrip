using System;

namespace GoNTrip.ServerInteraction.ModelFieldAttributes
{
    public class ExportField : Attribute { }

    public class SignUpField : ExportField { }
    public class LogInField : ExportField { }
    public class GetProfileField : ExportField { }
    public class UpdateProfileField : ExportField { }
}
