namespace GYM_ITM.Models.Observer
{
    public interface INotifier
    {
        Task<string> NotificarSuscriptores();

        void AñadirSuscriptor(Usuario suscriptor);

        void ElimminarSuscriptor(Usuario suscriptor);
    }
}
