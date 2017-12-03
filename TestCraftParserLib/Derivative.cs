namespace TestCraftParserLib
{
    public class Derivative<T>
    {
        public Derivative(T value, T delta)
        {
            Value = value;
            Delta = delta;
        }
        public T Value { get; }
        public T Delta { get; }
    }
}