using System.Collections.Generic;
using System.IO;

namespace Mustache
{
    /// <summary>
    /// Defines a tag that outputs the current index within an each loop.
    /// </summary>
    internal sealed class BeforeAppendTagDefinition : ContentTagDefinition
    {
        private const string beforeAppend = "beforeappendtext";
        private static readonly TagParameter beforeAppendParameter = new TagParameter(beforeAppend) { IsRequired = true };

        /// <summary>
        /// Initializes a new instance of an IndexTagDefinition.
        /// </summary>
        public BeforeAppendTagDefinition()
                    : base("beforeappend")
        {
        }

        public override IEnumerable<NestedContext> GetChildContext(TextWriter writer, Scope keyScope, Dictionary<string, object> arguments, Scope contextScope)
        {
            object appendable;
            if (contextScope.TryFind("beforeappend", out appendable))
            {
                if (appendable.ToString().ToLowerInvariant() != "true")
                    return new List<NestedContext>();
                return base.GetChildContext(writer, keyScope, arguments, contextScope);
            }
            return base.GetChildContext(writer, keyScope, arguments, contextScope);

        }

        public override IEnumerable<TagParameter> GetChildContextParameters()
        {
            return new List<TagParameter>() { beforeAppendParameter };
        }
    }
}
