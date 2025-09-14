using St8Ment.States.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace St8Ment;

public readonly struct StateId : IEquatable<StateId>
{
    public static readonly StateId Empty = new();

    #pragma warning disable IDE0290 // Use primary constructor
    public StateId(string value) => this.Value = Verify(value);

    private static string Verify(string id) => !StateRegexes.StateId().IsMatch(id)
        ? throw new InvalidCastException($"StateId '{id}' is not valid. Must be uppercase and use '_' or '-' as separator.")
        : id;

    public string Value { get; }

    public override string ToString() => this.Value;

    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is StateId other && this.Value == other.Value || 
        obj is string str && str == this.Value;

    public override int GetHashCode() => this.Value.GetHashCode();

    public bool Equals(StateId other) => this.Value == other.Value;

    public static bool operator ==(StateId left, StateId right) => left.Equals(right);

    public static bool operator !=(StateId left, StateId right) => !(left == right);

    public static implicit operator StateId(string value) => new(value);

    public static implicit operator string(StateId stateId) => stateId.Value;
}