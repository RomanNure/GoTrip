﻿using System;

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
    public class JoinPrepareField : QueryField { public JoinPrepareField(string name = null) : base(name) { } }
    public class CheckTourJoinAbilityField : QueryField { public CheckTourJoinAbilityField(string name = null) : base(name) { } }
    public class CheckTourGuidingAbilityField : QueryField { public CheckTourGuidingAbilityField(string name = null) : base(name) { } }
    public class GetTourMembersField : QueryField { public GetTourMembersField(string name = null) : base(name) { } }
    public class AddGuideField : QueryField { public AddGuideField(string name = null) : base(name) { } }
    public class PayField : QueryField { public PayField(string name = null) : base(name) { } }
    public class GetNotificationsField : QueryField { public GetNotificationsField(string name = null) : base(name) { } }
    public class CheckJoinStatusField : QueryField { public CheckJoinStatusField(string name = null) : base(name) { } }
    public class SeeNotificationField : QueryField { public SeeNotificationField(string name = null) : base(name) { } }
    public class AcceptNotificationField : QueryField { public AcceptNotificationField(string name = null) : base(name) { } }
    public class DeleteNotificationField : QueryField { public DeleteNotificationField(string name = null) : base(name) { } }
    public class RefuseNotificationField : QueryField { public RefuseNotificationField(string name = null) : base(name) { } }
    public class GetTicketField : QueryField { public GetTicketField(string name = null) : base(name) { } }
    public class OfferGuidingField : QueryField { public OfferGuidingField(string name = null) : base(name) { } }
    public class CheckTicketField : QueryField { public CheckTicketField(string name = null) : base(name) { } }
    public class NotificationPulseField : QueryField { public NotificationPulseField(string name = null) : base(name) { } }
    public class GetGuideById : QueryField { public GetGuideById(string name = null) : base(name) { } }
    public class FinishTour : QueryField { public FinishTour(string name = null) : base(name) { } }
    public class GetParticipatingStatus : QueryField { public GetParticipatingStatus(string name = null) : base(name) { } }
    public class GetTourAvgRating : QueryField { public GetTourAvgRating(string name = null) : base(name) { } }
    public class GetGuideAvgRating : QueryField { public GetGuideAvgRating(string name = null) : base(name) { } }
    public class CustomTourCreate : QueryField { public CustomTourCreate(string name = null) : base(name) { } }
}
