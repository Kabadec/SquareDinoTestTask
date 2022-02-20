namespace Scripts.Utils.Observer
{
    public interface IObserver
    {
        void TakeNotify(ISubject subject);
    }
}