using System.Security.Claims;
using RiskAnalyzer.Authorization;
using Xunit;

namespace RiskAnalyzer.Tests;

/// <summary>
/// Unit тестове за правилото „кой може да изтрива“ — без база данни и уеб сървър.
/// </summary>
public sealed class DeleteAuthorizationTests
{
    private static ClaimsPrincipal Principal(string userId, bool isAdmin)
    {
        var claims = new List<Claim>();
        if (!string.IsNullOrEmpty(userId))
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));

        if (isAdmin)
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));

        var identity = new ClaimsIdentity(claims, authenticationType: "Test");
        return new ClaimsPrincipal(identity);
    }

    [Fact]
    public void Admin_May_Delete_Even_If_No_Owner_Assigned_To_Record()
    {
        var admin = Principal(userId: "admin-guid", isAdmin: true);
        Assert.True(DeleteAuthorization.UserMayDelete(admin, ownerUserId: null));
    }

    [Fact]
    public void Admin_May_Delete_Some_One_Else_s_Record()
    {
        var admin = Principal(userId: "admin-guid", isAdmin: true);
        Assert.True(DeleteAuthorization.UserMayDelete(admin, ownerUserId: "other-user"));
    }

    [Fact]
    public void Owner_May_Delete_Own_Record_When_UserId_Matches()
    {
        var user = Principal(userId: "user-aaa", isAdmin: false);
        Assert.True(DeleteAuthorization.UserMayDelete(user, ownerUserId: "user-aaa"));
    }

    [Fact]
    public void NonOwner_Must_Not_Delete_AnotherUser_s_Record()
    {
        var user = Principal(userId: "user-bbb", isAdmin: false);
        Assert.False(DeleteAuthorization.UserMayDelete(user, ownerUserId: "user-aaa"));
    }

    [Fact]
    public void Ordinary_User_Must_Not_Delete_Legacy_Unowned_Record()
    {
        var user = Principal(userId: "user-ccc", isAdmin: false);
        Assert.False(DeleteAuthorization.UserMayDelete(user, ownerUserId: null));
    }

    [Fact]
    public void Without_NameIdentifier_Claim_Delete_Is_Denied()
    {
        var anonymous = Principal(userId: "", isAdmin: false);
        Assert.False(DeleteAuthorization.UserMayDelete(anonymous, ownerUserId: "any-one"));
    }

    [Fact]
    public void UserMayEdit_Matches_UserMayDelete_For_Owner_And_NonOwner()
    {
        var user = Principal(userId: "user-aaa", isAdmin: false);
        Assert.Equal(
            DeleteAuthorization.UserMayDelete(user, "user-aaa"),
            DeleteAuthorization.UserMayEdit(user, "user-aaa"));
        Assert.Equal(
            DeleteAuthorization.UserMayDelete(user, "other"),
            DeleteAuthorization.UserMayEdit(user, "other"));
    }
}
