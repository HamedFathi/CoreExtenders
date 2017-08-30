using System.Collections.Generic;
using System.IO;

namespace Mustache
{
    public class CommentTagDefinition : ContentTagDefinition
    {
        private const string commentText = "commenttext";
        private static readonly TagParameter commentParameter = new TagParameter(commentText) { IsRequired = true };

        /// <summary>
        /// Initializes a new instance of an IndexTagDefinition.
        /// </summary>
        public CommentTagDefinition()
                    : base("comment")
        {
        }

        public override IEnumerable<NestedContext> GetChildContext(TextWriter writer, Scope keyScope, Dictionary<string, object> arguments, Scope contextScope)
        {
            return new List<NestedContext>();
        }

        public override IEnumerable<TagParameter> GetChildContextParameters()
        {
            return new List<TagParameter>() { commentParameter };
        }
    }
}
