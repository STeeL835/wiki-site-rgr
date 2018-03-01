using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Ganss.XSS;
using Markdig;
using Microsoft.Security.Application;

namespace WikiSite.PL.ASP.Classes
{
    public static class HtmlHelperExtentions
    {
	    static HtmlHelperExtentions()
	    {
		    Sanitizer = new HtmlSanitizer();
		    Sanitizer.AllowedAttributes.Add("class"); // I know, classjacking, but bootstrap!
		    Sanitizer.AllowedTags.Add("iframe"); // Youtube videos
	    }
        public class ResourceInclude
        {
            public string Path { get; set; }
            public int Priority { get; set; }
        }
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

        public static string RequireLocalScript(this HtmlHelper html, string code, int priority = 0)
        {
            var requiredScripts = HttpContext.Current.Items["RequiredLocalScripts"] as List<ResourceInclude>;
            if (requiredScripts == null) HttpContext.Current.Items["RequiredLocalScripts"] = requiredScripts = new List<ResourceInclude>();
            if (!requiredScripts.Any(i => i.Path == code)) requiredScripts.Add(new ResourceInclude() { Path = code, Priority = priority });
            return null;
        }

        public static HtmlString EmitRequiredLocalScripts(this HtmlHelper html)
        {
            var requiredScripts = HttpContext.Current.Items["RequiredLocalScripts"] as List<ResourceInclude>;
            if (requiredScripts == null) return null;
            StringBuilder sb = new StringBuilder();
            foreach (var item in requiredScripts.OrderBy(i => i.Priority))
            {
                sb.AppendFormat("<script>{0}</script>\n", item.Path);
            }
            return new HtmlString(sb.ToString());
        }

		#region MarkdownHelper

	    private static readonly HtmlSanitizer Sanitizer;

	    private static MarkdownPipeline Pipeline { get; } = new MarkdownPipelineBuilder()
		                                                             .UseAdvancedExtensions()
		                                                             .UseBootstrap()
		                                                             .UseEmojiAndSmiley()
		                                                             .Build();

		public static IHtmlString Markdown(this HtmlHelper html, string markText)
		{
			return html.Raw(Sanitizer.Sanitize(Markdig.Markdown.ToHtml(markText, Pipeline)));
		}

	    #endregion
	}
}