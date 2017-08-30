using System.Collections.Generic;

namespace Mustache
{
    public class IsNullOrEmptyTagDefinition : ContentTagDefinition
    {
        private const string conditionParameter = "condition";

        public IsNullOrEmptyTagDefinition()
                            : base("IsNullOrEmpty")
        { }

        public override IEnumerable<TagParameter> GetChildContextParameters()
        {
            return new TagParameter[0];
        }

        protected override bool GetIsContextSensitive()
        {
            return false;
        }

        protected override IEnumerable<TagParameter> GetParameters()
        {
            return new TagParameter[] { new TagParameter(conditionParameter) { IsRequired = true } };
        }

        private bool isConditionSatisfied(object condition)
        {
            if (condition == null)
            {
                return true;
            }

            return condition is string ? string.IsNullOrEmpty(condition as string) : false;
        }

        public override bool ShouldGeneratePrimaryGroup(Dictionary<string, object> arguments)
        {
            object condition = arguments[conditionParameter];
            return isConditionSatisfied(condition);
        }
    }
}
