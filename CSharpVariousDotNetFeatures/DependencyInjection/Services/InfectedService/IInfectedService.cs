using DependencyInjection.Model;

namespace DependencyInjection.Services.InfectedService
{
    public interface IInfectedService
    {
        int GetInfectedCount();

        List<Infected> GetInfectedList();

        void IncreaseInfected();
    }
}
