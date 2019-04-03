using System;

namespace OpenSBP {

    public enum EventType {
        JumpTarget,
        Assignment,
        Command,
        Empty
    }
    public struct OnInputInfo {
        public EventType type;
        public string jumpTarget;      // type == JumpTarget.
        public string assignmentName;  // type == Assignment
        public Value assignmentValue;  // |
        public string commandStr;      // type == Command
    }
}
