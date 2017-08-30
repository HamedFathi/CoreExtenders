using System.Collections.Generic;
using System.IO;

namespace Mustache
{
    /// <summary>
    /// Defines a tag that outputs the current index within an each loop.
    /// </summary>
    internal sealed class AfterAppendTagDefinition : ContentTagDefinition
    {
        private const string afterAppend = "afterappendtext";
        private static readonly TagParameter afterAppendParameter = new TagParameter(afterAppend) { IsRequired = true };

        /// <summary>
        /// Initializes a new instance of an IndexTagDefinition.
        /// </summary>
        public AfterAppendTagDefinition()
                    : base("afterappend", true)
        {
        }

        public override IEnumerable<NestedContext> GetChildContext(TextWriter writer, Scope keyScope, Dictionary<string, object> arguments, Scope contextScope)
        {
            object appendable;
            if (contextScope.TryFind("afterappend", out appendable))
            {
                if (appendable.ToString().ToLowerInvariant() != "true")
                    return new List<NestedContext>();
                return base.GetChildContext(writer, keyScope, arguments, contextScope);
            }
            return base.GetChildContext(writer, keyScope, arguments, contextScope);

        }

        public override IEnumerable<TagParameter> GetChildContextParameters()
        {
            return new List<TagParameter>() { afterAppendParameter };
        }
    }
}
