using Mandrill.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Mandrill.Model
{
    [JsonConverter(typeof(MandrillHeaderValueConverter))]
    public readonly struct MandrillHeaderValue : IEquatable<MandrillHeaderValue>
    {
        private readonly string[] _values;

        public MandrillHeaderValue(string value)
        {
            _values = value != null ? [value] : [];
        }

        public MandrillHeaderValue(IEnumerable<string> values)
        {
            _values = values?.ToArray() ?? [];
        }

        public string Value => _values?.Length > 0 ? _values[0] : null;
        public IReadOnlyList<string> Values => (IReadOnlyList<string>)_values ?? [];
        public bool HasMultipleValues => _values?.Length > 1;

        public static implicit operator MandrillHeaderValue(string value) => new(value);
        public static implicit operator MandrillHeaderValue(string[] values) => new(values);
        public static implicit operator string(MandrillHeaderValue header) => header.Value;

        public override string ToString() => Value ?? string.Empty;

        public bool Equals(MandrillHeaderValue other)
        {
            var a = _values ?? [];
            var b = other._values ?? [];
            return a.SequenceEqual(b);
        }

        public override bool Equals(object obj) => obj is MandrillHeaderValue other && Equals(other);

        public override int GetHashCode()
        {
            if (_values == null || _values.Length == 0) return 0;
            var hash = new System.HashCode();
            foreach (var v in _values) hash.Add(v);
            return hash.ToHashCode();
        }
    }
}
