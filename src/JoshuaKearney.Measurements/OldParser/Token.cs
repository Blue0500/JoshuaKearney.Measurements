namespace JoshuaKearney.Measurements.Parser {

    internal class Token {
        public string Value { get; }

        public Token(string value) {
            this.Value = value;
        }

        public override string ToString() {
            return this.Value;
        }
    }
}