using HtmlAgilityPack;
using Markdig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBlog.Services.ServicesHelper
{
    public static class MarkdownHelper
    {
        public static string ExtractPlainTextFromMarkdown(string markdown, int maxLength = 200)
        {
            if (string.IsNullOrWhiteSpace(markdown))
                return string.Empty;

            // 转换 Markdown 为 HTML
            var html = Markdown.ToHtml(markdown);

            // 直接提取所有纯文本（移除所有HTML标签）
            string plainText = Regex.Replace(html, "<.*?>", " ");

            // 规范化空白（合并连续空白字符）
            plainText = Regex.Replace(plainText, @"\s+", " ").Trim();

            // 处理空文本
            if (string.IsNullOrWhiteSpace(plainText))
                return string.Empty;

            // 智能截断（避免截断单词）
            if (plainText.Length > maxLength)
            {
                // 查找最近的单词边界
                int lastSpace = plainText.LastIndexOf(' ', maxLength - 1);
                int safeLength = (lastSpace > 0) ? lastSpace : maxLength;

                // 确保截断位置有效
                safeLength = Math.Min(safeLength, plainText.Length);
                plainText = plainText.Substring(0, safeLength).TrimEnd() + "...";
            }

            return plainText;
        }

        /// <summary>
        /// 将 Markdown 文本转换为 HTML
        /// </summary>
        public static string ConvertMarkdownToHtml(this string markdown)
        {
            if (string.IsNullOrWhiteSpace(markdown))
                return string.Empty;

            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();

            var html = Markdown.ToHtml(markdown, pipeline);

            return html;
        }
    }
}
