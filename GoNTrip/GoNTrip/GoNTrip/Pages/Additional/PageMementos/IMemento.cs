namespace GoNTrip.Pages.Additional.PageMementos
{
    public interface IMemento<T>
    {
        void Save(T obj, params object[] restoreCtorParams);
        T Restore();
    }
}
