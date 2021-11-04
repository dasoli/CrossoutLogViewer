namespace CrossoutLogView.Common
{
    public interface IMergable<T>
    {
        public T Merge(T other);
    }
}