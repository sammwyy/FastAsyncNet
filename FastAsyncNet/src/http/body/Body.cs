namespace FastAsyncNet
{
    public class Body
    {
        private string _raw;

        public Body()
        {
            this._raw = "";
        }

        public Body(string value)
        {
            this._raw = value;
        }

        public void Append(string value)
        {
            this._raw = this._raw + value;
        }

        public void AppendLine(string value)
        {
            this.Append(value + "\n");
        }

        public virtual string GetAsString()
        {
            return this._raw;
        }
    }
}