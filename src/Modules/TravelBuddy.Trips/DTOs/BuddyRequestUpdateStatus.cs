using System.Runtime.Serialization;

namespace TravelBuddy.Trips.DTOs;
public enum BuddyRequestUpdateStatus
{
    [EnumMember(Value = "accepted")]
    Accepted,

    [EnumMember(Value = "rejected")]
    Rejected,
}