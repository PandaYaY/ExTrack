using ProverkaCheka.Dto;
using SqlKata.Execution;

namespace ExTrack.Repositories;

public interface ICheckRepository
{
    Task AddCheckInfo(CheckInfoJsonDto checkInfo);
}

public class CheckRepository(QueryFactory db) : ICheckRepository
{
    public async Task AddCheckInfo(CheckInfoJsonDto checkInfo)
    {
        var             query      = db.Query("public.create_shop");
        Console.WriteLine(query.);
    }
}
