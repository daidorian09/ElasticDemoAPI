using ElasticSearchAPITest.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElasticSearchAPITest.Data.Interfaces
{
    public interface IStateService
    {
        Task<ICollection<State>> GetStatesAsync();
    }
}
