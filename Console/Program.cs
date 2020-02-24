using System;
using System.Collections.Generic;
using System.IO;
using IdeaGenerator;
using Logic;
using Logic.Templating;

namespace ConsoleGUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();

            program.Menu(program.Config());
        }

        private Dictionary<string, Template> Config()
        {
            var folderPath = "TemplateResources";

            Console.WriteLine($"{Path.Combine(Directory.GetCurrentDirectory(), folderPath)}");
            Console.WriteLine("Enter a different path for the templates forlder or leave it blank to continue with the default path.");
            var answer = Console.ReadLine();

            if (answer != string.Empty)
                folderPath = answer;

            return TemplateSerializer.ReadFolder(folderPath);
        }

        private void Menu(Dictionary<string, Template> templates)
        {
            if (templates.Count > 0)
            {
                if (templates.TryGetValue("_EntryPoint", out var entryPoint) != false)
                {
                    var rand = new Random(0);

                    var cachedTemplates = new Dictionary<string, string>();

                    if (templates.TryGetValue("_Init", out var initTemplate))
                    {
                        cachedTemplates.Initialize(templates, initTemplate, rand);
                    }
                    else
                    {
                        Console.WriteLine("_Init could not be found.");
                        return;
                    }

                    var answer = "y";
                    while (answer == "y")
                    {
                        var story = entryPoint.Generate(rand);

                        Console.WriteLine($"\n{ story.FillAll(templates, cachedTemplates, rand) }\n");

                        Console.WriteLine("Want to hear another great idea? (y to repeat)");
                        answer = Console.ReadLine();

                        cachedTemplates.Initialize(templates, initTemplate, rand);
                    }
                }
                else
                {
                    Console.WriteLine("Could not found any template with the tag 'Start'.\n" +
                        "Without this template the program does not know where to start.");
                }
            }
            else
            {
                Console.WriteLine("Could not found any template at the given path.");
            }

            Console.WriteLine("\nHave a nice day! :)\n\n\n");
        }
    }
}