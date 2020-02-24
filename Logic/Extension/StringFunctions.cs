using Logic;
using System;
using System.Collections.Generic;
using Logic.Templating;

namespace IdeaGenerator
{
    public static class StringFunctions
    {
        public static Dictionary<string, string> Initialize(this Dictionary<string, string> dictionary, Dictionary<string, Template> templates, 
            Template initTemplate, Random rand)
        {
            dictionary.Clear();

            foreach (var option in initTemplate.options)
            {
                dictionary.Add(option, option.FillAll(templates, dictionary, rand));
            }

            return dictionary;
        }

        public static string FillAll(this string str, Dictionary<string, Template> templates, Dictionary<string, string> cacheTemplates, Random rand)
        {
            if (str.Contains("["))
            {
                str = str.FillRandomTag(rand);

                return str.FillAll(templates, cacheTemplates, rand);
            }
            else if (str.Contains("{"))
            {
                str = str.FillCachedTag("{", "}", templates, cacheTemplates, rand, delete: true);

                return str.FillAll(templates, cacheTemplates, rand);
            }
            else if (str.Contains("$"))
            {
                str = str.FillCachedTag("$", "$", templates, cacheTemplates, rand);

                return str.FillAll(templates, cacheTemplates, rand);
            }
            else if(str.Contains("@"))
            {
                str = str.FillTag("@", "@", templates, rand);

                return str.FillAll(templates, cacheTemplates, rand);
            }

            return str;
        }

        public static (string tag, int startIndex, int endIndex) ReadTagInfo(this string str, string startTag, string endTag)
        {
            var startIndex = str.IndexOf(startTag);
            var endIndex = str.IndexOf(endTag, startIndex + startTag.Length);
            var tag = str.Substring(startIndex + startTag.Length, endIndex - (startIndex + startTag.Length));

            return (tag, startIndex, endIndex + endTag.Length);
        }

        public static string FillTag(this string str, string startTag, string endTag, Dictionary<string, Template> templates, Random rand)
        {
            var (tag, startIndex, endIndex) =  str.ReadTagInfo(startTag, endTag);

            if (templates.TryGetValue(tag, out var attribute))
            {
                str = str.ReplaceAt(startIndex, endIndex, attribute.Generate(rand));
            }
            else
            {
                str = str.ReplaceAt(startIndex, endIndex, $"*'{ tag }' MISSING*");
            }

            return str;
        }

        public static string FillCachedTag(this string str, string startTag, string endTag, 
            Dictionary<string, Template> templates, Dictionary<string, string> cache, Random rand, bool delete = false)
        {
            var (tag, startIndex, endIndex) = str.ReadTagInfo(startTag, endTag);

            var colonIndex = tag.IndexOf(':');
            var cahedTagName = tag.Substring(0, colonIndex);
            tag = tag.Substring(colonIndex + 1, tag.Length - colonIndex - 1);
            
            if (cache.TryGetValue(cahedTagName, out var cached))
            {
                str = str.ReplaceAt(startIndex, endIndex, delete ? string.Empty : cached);
            }
            else
            {
                cached = tag.FillAll(templates, cache, rand);
                cache.Add(cahedTagName, cached);

                str = str.ReplaceAt(startIndex, endIndex, delete ? string.Empty : cached);
            }

            return str;
        }

        public static string FillRandomTag(this string str, Random rand)
        {
            var (tag, startIndex, endIndex) = str.ReadTagInfo("[", "]");

            var options = tag.Split('|');
            var chosedOption = options[(int)Math.Round(rand.NextDouble() * (options.Length - 1))];

            return str.ReplaceAt(startIndex, endIndex, chosedOption);
        }

        public static string ReplaceAt(this string str, int startIndex, int endIndex, string replace)
        {
            return str.Remove(startIndex, endIndex - startIndex).Insert(startIndex, replace);
        }
    }
}
