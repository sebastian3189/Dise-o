namespace GYM_ITM.Models.Observer
{
    public interface INotifier
    {
        Task<string> NotificarSuscriptores(int idHorario);

        void AñadirSuscriptor(Usuario suscriptor);

        void ElimminarSuscriptor(Usuario suscriptor);
    }
}
