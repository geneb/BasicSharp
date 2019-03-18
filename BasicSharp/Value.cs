﻿using System;

namespace OpenSBP {
    public enum ValueType {
        Real,
        String
    }

    public struct Value {
        public static readonly Value Zero = new Value(0);
        public ValueType Type { get; set; }

        public double Real { get; set; }
        public string String { get; set; }

        public Value(double real) : this() {
            this.Type = ValueType.Real;
            this.Real = real;
        }

        public Value(string str)
            : this() {
            this.Type = ValueType.String;
            this.String = str;
        }

        public Value Convert(ValueType type) {
            if (this.Type != type) {
                switch (type) {
                    case ValueType.Real:
                        this.Real = double.Parse(this.String);
                        this.Type = ValueType.Real;
                        break;
                    case ValueType.String:
                        this.String = this.Real.ToString();
                        this.Type = ValueType.String;
                        break;
                }
            }
            return this;
        }

        public Value BinOp(Value b, Token tok) {
            Value a = this;
            if (a.Type != b.Type) {
                if (a.Type > b.Type)
                    b = b.Convert(a.Type);
                else
                    a = a.Convert(b.Type);
            }

            if (tok == Token.Ampersand) {
                // & should only be permitted with strings!
                if ((a.Type != ValueType.String) &&
                    (b.Type != ValueType.String)) {
                    throw new Exception("'&' can only be used to concatenate strings!");
                } else {
                    return new Value(a.String + b.String);
                }
            } else if (tok == Token.Plus) {
                if (a.Type == ValueType.Real)
                    return new Value(a.Real + b.Real);
                else
                    return new Value(a.String + b.String);
            } else if (tok == Token.Equal) {
                if (a.Type == ValueType.Real)
                    return new Value(a.Real == b.Real ? 1 : 0);
                else
                    return new Value(a.String == b.String ? 1 : 0);
            } else if (tok == Token.NotEqual) {
                if (a.Type == ValueType.Real)
                    return new Value(a.Real == b.Real ? 0 : 1);
                else
                    return new Value(a.String == b.String ? 0 : 1);
            } else {
                if (a.Type == ValueType.String)
                    throw new Exception("Cannot do binop on strings(except +).");  //TODO Needs more detail.

                switch (tok) {
                    // TODO this area may be a good place to parse the extended OpenSBP operations.
                    case Token.Minus: return new Value(a.Real - b.Real);
                    case Token.Asterisk: return new Value(a.Real * b.Real);
                    case Token.Slash: return new Value(a.Real / b.Real);
                    case Token.Caret: return new Value(Math.Pow(a.Real, b.Real));
                    case Token.Less: return new Value(a.Real < b.Real ? 1 : 0);
                    case Token.More: return new Value(a.Real > b.Real ? 1 : 0);
                    case Token.LessEqual: return new Value(a.Real <= b.Real ? 1 : 0);
                    case Token.MoreEqual: return new Value(a.Real >= b.Real ? 1 : 0);
                }
            }
            throw new Exception("Unknown binop");  //TODO this could be more clear.
        }

        public override string ToString() {
            if (this.Type == ValueType.Real)
                return this.Real.ToString();
            return this.String;
        }
    }
}
