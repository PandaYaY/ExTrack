using ExTrack.Repositories;
using ProverkaCheka.Client;
using ProverkaCheka.Dto;

namespace ExTrack.Service.CheckService;

public interface ICheckService
{
    Task GetCheckInfoAsync(GetCheckInfoRequestDto dto);
}

public class CheckService(IProverkaChekaClient proverkaChekaClient, ICheckRepository checkRepository) : ICheckService
{
    public async Task GetCheckInfoAsync(GetCheckInfoRequestDto dto)
    {
        var checkInfo = await proverkaChekaClient.GetCheckInfo(dto);
        // TODO StatusCode check!
        await checkRepository.AddCheckInfo(checkInfo.Data.Json);
    }
}
