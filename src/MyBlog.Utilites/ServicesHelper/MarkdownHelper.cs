using HtmlAgilityPack;
using Markdig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Services.ServicesHelper
{
    public static class MarkdownHelper
    {
        public static string ExtractPlainTextFromMarkdown(string markdown, int maxLength = 200)
        {
            var html = Markdown.ToHtml(markdown ?? "");

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var nodesToRemove = doc.DocumentNode.SelectNodes("//img | //pre | //code");
            if (nodesToRemove != null)
            {
                foreach (var node in nodesToRemove)
                {
                    node.Remove();
                }
            }

            var text = doc.DocumentNode.InnerText;

            return string.IsNullOrWhiteSpace(text)
                ? string.Empty
                : text.Trim().Length > maxLength ? text.Trim().Substring(0, maxLength) + "..." : text.Trim();
        }
    }
}
