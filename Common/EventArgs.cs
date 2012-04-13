namespace Common
{
    public class EventArgs<T> : System.EventArgs
    {
        public T Data { get; private set; }

        public EventArgs(T data)
        {
            Data = data;
        }
    }
}