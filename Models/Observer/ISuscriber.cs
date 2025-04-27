namespace GYM_ITM.Models.Observer
{
    public interface ISuscriber
    {
        Task<string> NotificarUsuario(int id);
    }
}
