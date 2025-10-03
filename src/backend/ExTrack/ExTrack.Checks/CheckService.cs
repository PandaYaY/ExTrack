using System.Text.Json;
using ExTrack.Checks.Models;
using Microsoft.Extensions.Logging;
using ProverkaCheka.Client;
using ProverkaCheka.Dto;

namespace ExTrack.Checks;

public interface IChecksService
{
    Task GetCheckInfoAsync(GetCheckInfoRequestDto dto);
}

public class ChecksService(
    ILogger<ChecksService> logger,
    IProverkaChekaClient   proverkaChekaClient,
    IChecksRepository      checksRepository) : IChecksService
{
    public async Task GetCheckInfoAsync(GetCheckInfoRequestDto dto)
    {
        var getCheckInfoResponse = await proverkaChekaClient.GetCheckInfo(dto);
        if (getCheckInfoResponse.StatusCode is not ScanResultEnum.Success)
        {
            logger.LogWarning("GetCheckInfo returned error code: {StatusCode}, {response}",
                              getCheckInfoResponse.StatusCode, JsonSerializer.Serialize(getCheckInfoResponse));
            throw new ArgumentException("Неизвестная ошибка сервиса \"Proverka Cheka\"", nameof(dto));
        }

        var checkInfo = ConvertToCheckInfo(getCheckInfoResponse);
        await checksRepository.AddCheckInfo(checkInfo);
    }

    private CheckInfo ConvertToCheckInfo(GetCheckInfoResponseDto response)
    {
        var data = response.Data.JsonData;
        var products = data.Items.Select(item => new CheckProduct(item.Name, item.Price, item.Sum, item.Quantity))
                           .ToList();
        return new CheckInfo(data.RetailPlace, data.RetailPlaceAddress, data.Timestamp, data.Sum, products);
    }
}
