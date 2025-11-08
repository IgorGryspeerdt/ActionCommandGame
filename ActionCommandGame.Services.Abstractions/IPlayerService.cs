using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IPlayerService
    {
        Task<ServiceResult<PlayerResult>> Get(int id, int userId);
        Task<ServiceResult<IList<PlayerResult>>> Find(PlayerFilter? filter);
        Task<ServiceResult<PlayerResult>> Create(string name, int userId);
        Task<ServiceResult<PlayerResult>> Update(UpdatePlayerRequest model, int userId);
        Task<ServiceResult> Delete(int id, int userId);
    }
}
