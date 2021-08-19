namespace FastAsyncNet
{
    public abstract class Body
    {

        public readonly string Raw;

        public Body(string value)
        {
            this.Raw = value;
        }

        public virtual string GetAsString()
        {
            return this.Raw;
        }
    }
}