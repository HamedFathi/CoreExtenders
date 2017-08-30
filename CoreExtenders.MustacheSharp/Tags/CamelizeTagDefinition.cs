using Humanizer;
using System.Collections.Generic;
using System.IO;

namespace Mustache
{
    public class CamelizeTagDefinition : InlineTagDefinition
    {
        public CamelizeTagDefinition()
                    : base("camelize")
        {
        }

        protected override IEnumerable<TagParameter> GetParameters()
        {
            return new[] { new TagParameter("param") { IsRequired = true } };
        }

        public override void GetText(TextWriter writer, Dictionary<string, object> arguments, Scope context)
        {
            writer.Write(arguments["param"].ToString().Camelize());
        }
    }
}
