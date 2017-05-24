using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WikiSite.PL.ASP.Classes
{
    public static class HtmlHelperExtentions
    {
        public static string RequireStyle(this HtmlHelper html, string path, int priority = 0)
        {
            var requiredStyle = HttpContext.Current.Items["RequiredStyle"] as List<ResourceInclude>;
            if (requiredStyle == null) HttpContext.Current.Items["RequiredStyle"] = requiredStyle = new List<ResourceInclude>();
            if (!requiredStyle.Any(i => i.Path == path)) requiredStyle.Add(new ResourceInclude() { Path = path, Priority = priority });
            return null;
        }

        public static HtmlString EmitRequiredStyles(this HtmlHelper html)
        {
            var requiredStyle = HttpContext.Current.Items["RequiredStyle"] as List<ResourceInclude>;
            if (requiredStyle == null) return null;
            StringBuilder sb = new StringBuilder();
            foreach (var item in requiredStyle.OrderBy(i => i.Priority))
            {
                sb.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\">\n", item.Path);
            }
            return new HtmlString(sb.ToString());
        }
        public static string RequireScript(this HtmlHelper html, string path, int priority = 0)
        {
            var requiredScripts = HttpContext.Current.Items["RequiredScripts"] as List<ResourceInclude>;
            if (requiredScripts == null) HttpContext.Current.Items["RequiredScripts"] = requiredScripts = new List<ResourceInclude>();
            if (!requiredScripts.Any(i => i.Path == path)) requiredScripts.Add(new ResourceInclude() { Path = path, Priority = priority });
            return null;
        }

        public static HtmlString EmitRequiredScripts(this HtmlHelper html)
        {
            var requiredScripts = HttpContext.Current.Items["RequiredScripts"] as List<ResourceInclude>;
            if (requiredScripts == null) return null;
            StringBuilder sb = new StringBuilder();
            foreach (var item in requiredScripts.OrderBy(i => i.Priority))
            {
                sb.AppendFormat("<script src=\"{0}\" type=\"text/javascript\"></script>\n", item.Path);
            }
            return new HtmlString(sb.ToString());
        }
        public class ResourceInclude
        {
            public string Path { get; set; }
            public int Priority { get; set; }
        }
    }
}