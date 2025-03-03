namespace TeamChat.Server.Infrastructure;

public static class HttpExtensions
{
    public static int? GetUserId(this HttpContext ctx)
    {
        var nameIdentifier = ctx.User.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;
        if (int.TryParse(nameIdentifier, out var userId) == false)
        {
            return null;
        }
        return userId;
    }
}
