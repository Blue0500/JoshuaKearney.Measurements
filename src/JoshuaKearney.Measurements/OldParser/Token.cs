namespace JoshuaKearney.Measurements.OldParser {

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