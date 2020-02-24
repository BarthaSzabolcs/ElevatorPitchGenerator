using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Templating
{
    public class Template
    {
        public string tag;
        public List<string> options;

        public string Generate(Random rand)
        {
            return options[(int)Math.Round(rand.NextDouble() * (options.Count - 1))];
        }
    }
}
