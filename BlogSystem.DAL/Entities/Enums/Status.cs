using System.Runtime.Serialization;

namespace BlogSystem.DAL.Entities.Enums
{
    public enum Status
    {
        [EnumMember(Value = "Published")]
        Published,
        [EnumMember(Value = "Draft")]
        Draft,
        [EnumMember(Value = "Archived")]
        Archived
    }
}
