namespace FistWeb.Data.Services
{
    public class LoadingService
    {
        private int _count = 0;
        public event Action? OnChange;

        public bool IsLoading => _count > 0;

        public void Begin()
        {
            Interlocked.Increment(ref _count);
            OnChange?.Invoke();
        }

        public void End()
        {
            if (Interlocked.Decrement(ref _count) < 0) Interlocked.Exchange(ref _count, 0);
            OnChange?.Invoke();
        }
    }
}
