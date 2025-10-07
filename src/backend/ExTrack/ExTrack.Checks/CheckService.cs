using System.Text.Json;
using ExTrack.Api.Dto.Checks;
using ExTrack.Checks.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProverkaCheka.Client;
using ProverkaCheka.Dto;

namespace ExTrack.Checks;

public interface IChecksService
{
    Task<CheckEntity?>      GetCheckById(int                  checkId);
    Task<List<CheckEntity>> GetUserChecks(int                 userId, int page, int perPage);
    Task<int>               GetCheckInfoAsync(GetCheckInfoDto checkInfoDto);
}

public class ChecksService(
    ILogger<ChecksService>                     logger,
    IProverkaChekaClient                       proverkaChekaClient,
    IChecksRepository                          checksRepository,
    IOptions<ProverkaChekaClientConfiguration> proverkaChekaConfig) : IChecksService
{
    private readonly string _accessToken = proverkaChekaConfig.Value.AccessToken;

    public async Task<CheckEntity?> GetCheckById(int checkId)
    {
        var check = await checksRepository.GetCheckById(checkId);
        return check;
    }

    public async Task<List<CheckEntity>> GetUserChecks(int userId, int page, int perPage)
    {
        var checks = await checksRepository.GetUserChecks(userId, page, perPage);
        return checks;
    }

    public async Task<int> GetCheckInfoAsync(GetCheckInfoDto checkInfoDto)
    {
        var dto                  = ConvertToGetCheckInfoRequestDto(checkInfoDto);
        var getCheckInfoResponse = await proverkaChekaClient.GetCheckInfo(dto);
        if (getCheckInfoResponse.StatusCode is not ScanResultEnum.Success)
        {
            logger.LogWarning("GetCheckInfo returned error code: {StatusCode}, {response}",
                              getCheckInfoResponse.StatusCode, JsonSerializer.Serialize(getCheckInfoResponse));
            throw new ArgumentException("Неизвестная ошибка сервиса \"Proverka Cheka\"", nameof(dto));
        }

        var checkInfo = ConvertToCheckInfo(getCheckInfoResponse);
        var checkId   = await checksRepository.AddCheckInfo(checkInfoDto.UserId, checkInfo);
        return checkId;
    }

    private static CheckInfo ConvertToCheckInfo(GetCheckInfoResponseDto response)
    {
        var data = response.Data.JsonData;
        var products =
            data.Items.ConvertAll(item => new CheckProduct(item.Name, item.Price / 100.0, item.Sum / 100.0,
                                                           item.Quantity));
        return new CheckInfo(data.RetailPlace, data.RetailPlaceAddress, data.Timestamp, data.Sum / 100.0, products);
    }

    private GetCheckInfoRequestDto ConvertToGetCheckInfoRequestDto(GetCheckInfoDto dto)
    {
        return new GetCheckInfoRequestDto(_accessToken, dto.FiscalStorageDeviceNumber, dto.FiscalDocumentNumber,
                                          dto.DocumentFiscalAttribute, dto.Timestamp, dto.OperationType, dto.Sum,
                                          IsQr: false);
    }
}
