using System.Net;
using System;

public class ThemeSetter
{
    public static string SetThemeClass(HttpContext context)
    {
        var mode = context.Request.Query["mode"].ToString();

        if (mode != "")
        {
            context.Response.Cookies.Append("mode", mode);
            return mode + "-mode";
        }

        context.Request.Cookies.TryGetValue("mode", out string? result);
        return (result ?? "light") + "-mode";
    }
}