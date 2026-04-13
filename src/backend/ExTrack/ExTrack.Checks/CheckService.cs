using System.Globalization;
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
    Task<CheckEntity>       GetCheckInfoAsync(GetCheckInfoDto checkInfoDto);
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

    public async Task<CheckEntity> GetCheckInfoAsync(GetCheckInfoDto checkInfoDto)
    {
        var existCheck = await checksRepository.GetCheckByParams(checkInfoDto);
        if (existCheck is not null)
        {
            throw new ArgumentException("Чек уже добавлен", nameof(checkInfoDto));
        }
        
        var dto                  = ConvertToGetCheckInfoRequestDto(checkInfoDto);
        var getCheckInfoResponse = await proverkaChekaClient.GetCheckInfo(dto);
        if (getCheckInfoResponse.StatusCode is not ScanResultEnum.Success)
        {
            logger.LogWarning("GetCheckInfo returned error code: {StatusCode}, {@Response}",
                              getCheckInfoResponse.StatusCode, getCheckInfoResponse);
            throw new ArgumentException("Неизвестная ошибка сервиса \"Proverka Cheka\"", nameof(checkInfoDto));
        }

        var checkInfo    = ConvertToCheckInfo(getCheckInfoResponse);
        var newCheckInfo = await checksRepository.AddCheckInfo(checkInfoDto.UserId, checkInfo);
        return newCheckInfo;
    }

    private static CheckInfo ConvertToCheckInfo(GetCheckInfoResponseDto response)
    {
        var requestData               = response.RequestInfo.Input;
        var fiscalStorageDeviceNumber = long.Parse(requestData.Fn, CultureInfo.InvariantCulture);
        var fiscalDocumentNumber      = long.Parse(requestData.Fd, CultureInfo.InvariantCulture);
        var documentFiscalAttribute   = long.Parse(requestData.Fp, CultureInfo.InvariantCulture);
        var operationType             = (OperationType)requestData.OperationType;

        var data = response.Data.JsonData;
        var products =
            data.Items.ConvertAll(item => new CheckProduct(item.Name, item.Price / 100.0, item.Sum / 100.0,
                                                           item.Quantity));
        return new CheckInfo(fiscalStorageDeviceNumber, fiscalDocumentNumber, documentFiscalAttribute, operationType,
                             data.RetailPlace, data.RetailPlaceAddress, data.Timestamp, data.Sum / 100.0, products);
    }

    private GetCheckInfoRequestDto ConvertToGetCheckInfoRequestDto(GetCheckInfoDto dto)
    {
        return new GetCheckInfoRequestDto(_accessToken, dto.FiscalStorageDeviceNumber, dto.FiscalDocumentNumber,
                                          dto.DocumentFiscalAttribute, dto.Timestamp, dto.OperationType, dto.Sum,
                                          IsQr: false);
    }
}
