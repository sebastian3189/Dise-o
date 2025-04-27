namespace GYM_ITM.Models.Observer
{
    public interface INotifier
    {
        void NotificarSuscriptores();

        string AñadirSuscriptor(ISuscriber suscriptor);

        string ElimminarSuscriptor(ISuscriber suscriptor);
    }
}
