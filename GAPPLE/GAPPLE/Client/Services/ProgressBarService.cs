namespace GAPPLE.Client.Services
{
    public class ProgressBarService
    {
        public event Action<double> OnUpdatePorcentaje;
        public event Action<string> OnUpdateTexto;
        public event Action OnOpen;
        public event Action OnClose;

        public void Open() => OnOpen?.Invoke();

        public void Update(double porcentaje) => OnUpdatePorcentaje?.Invoke(porcentaje);

        public void Update(int porcentaje) => OnUpdatePorcentaje?.Invoke(porcentaje);

        public void Update(string text) => OnUpdateTexto?.Invoke(text);

        public void Close() => OnClose?.Invoke();
    }
}
