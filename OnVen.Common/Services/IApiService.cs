using OnVen.Common.Responses;
using System.Threading.Tasks;

namespace OnVen.Common.Services
{
    public interface IApiService
    {
        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);
    }
}
