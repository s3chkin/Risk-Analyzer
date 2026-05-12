using System.Security.Claims;

namespace RiskAnalyzer.Authorization;

public static class DeleteAuthorization
{
    /// <summary>
    /// Администратор или потребителят, създал записа (owner id съвпада).
    /// Записи без собственик (legacy) могат да се трият/редактират само от админ.
    /// </summary>
    public static bool UserMayDelete(ClaimsPrincipal user, string? ownerUserId)
    {
        if (user.IsInRole("Admin"))
            return true;

        var uid = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(ownerUserId))
            return false;

        return string.Equals(ownerUserId, uid, StringComparison.Ordinal);
    }

    /// <inheritdoc cref="UserMayDelete"/>
    public static bool UserMayEdit(ClaimsPrincipal user, string? ownerUserId) =>
        UserMayDelete(user, ownerUserId);
}
