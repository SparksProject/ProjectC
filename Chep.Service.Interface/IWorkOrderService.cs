using System.Threading.Tasks;

namespace Chep.Service.Interface
{
    public interface IWorkOrderService
    {
        Task<string> SetWorkOrderMastersModel(int id);
    }
}