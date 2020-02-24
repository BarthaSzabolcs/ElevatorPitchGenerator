using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Logic.Templating
{
    public static class TemplateSerializer
    {
        public static Dictionary<string, Template> ReadFolder(string folderPath)
        {
            var result = new Dictionary<string, Template>();
            
            if (Directory.Exists(folderPath))
            {
                foreach (var path in Directory.GetFiles(folderPath))
                {
                    var template = ReadFile(path);

                    if (template != null)
                    {
                        result.Add(template.tag, template);
                    }
                }
            }

            return result;
        }

        static Template ReadFile(string filePath)
        {
            string json = File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<Template>(json);
        }

        public static void SaveToFile(Template template, string filePath)
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(template));
        }
    }
}
