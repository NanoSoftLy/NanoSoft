using System;

namespace NanoSoft.Identity
{
    public interface IIdentityResult<in TPermission>
    {
        Guid Id { get; }
        string Name { get; }
        string LoginName { get; }
        string Culture { get; }
        Guid GroupId { get; }
        string GroupName { get; }
        bool Can(TPermission permission, string type);
        bool Can(TPermission permission);
    }
}
