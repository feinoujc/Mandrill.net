using System;
using System.Collections.Generic;

namespace Mandrill.Model
{
    public class MandrillMergeVarContent : IEquatable<MandrillMergeVarContent>
    {
        public MandrillMergeVarContent()
        {
        }

        public MandrillMergeVarContent(string value)
        {
            ValueAsString = value;
        }

        public MandrillMergeVarContent(List<Dictionary<string, object>> value)
        {
            ValueAsArray = value;
        }

        public string ValueAsString { get; set; }

        public List<Dictionary<string, object>> ValueAsArray { get; set; }

        public bool Equals(MandrillMergeVarContent other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(ValueAsString, other.ValueAsString) && Equals(ValueAsArray, other.ValueAsArray);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((ValueAsString != null ? ValueAsString.GetHashCode() : 0)*397) ^ (ValueAsArray != null ? ValueAsArray.GetHashCode() : 0);
            }
        }

        public static implicit operator MandrillMergeVarContent(string value)
        {
            return new MandrillMergeVarContent(value);
        }

        public static implicit operator MandrillMergeVarContent(List<Dictionary<string, object>> value)
        {
            return new MandrillMergeVarContent(value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MandrillMergeVarContent) obj);
        }
    }
}