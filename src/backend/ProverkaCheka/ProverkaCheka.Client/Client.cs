using ProverkaCheka.Dto;
using Refit;

namespace ProverkaCheka.Client;

public interface IProverkaChekaClient
{
    [Post("/api/v1/check/get")]
    public Task<GetCheckInfoResponseDto> GetCheckInfo(GetCheckInfoRequestDto payload);

    /*
     * Строка формата t=20200924T1837&s=349.93&fn=9282440300682838&i=46534&fp=1273019065&n=1
     */
    [Post("/api/v1/check/get")]
    public Task<GetCheckInfoResponseDto> GetCheckInfo(string payload);
}
