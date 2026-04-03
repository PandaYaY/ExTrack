using System.Text.Json;
using Newtonsoft.Json.Linq;
using ProverkaCheka.Dto;

namespace ProverkaCheka.Test;

/// <summary>Общие данные для тестов сериализации</summary>
public sealed class SerializationTestData
{
    public JObject ResponseJson { get; }
    public GetCheckInfoRequestDto RequestDto { get; }
    public JsonSerializerOptions Options { get; } = new() { WriteIndented = true };

    public SerializationTestData()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Mock", "Response.json");
        var responseJson = File.ReadAllText(path);
        ResponseJson = JObject.Parse(responseJson);

        RequestDto = new GetCheckInfoRequestDto("testToken", 7380440800939112, 30641, 1310356009, "09.04.2025 20:07",
            OperationType.Prihod, 579.44, false);
    }
}

public class SerializationTests(SerializationTestData data, ITestOutputHelper output)
    : IClassFixture<SerializationTestData>
{
    #region Response Deserialization

    [Fact]
    public void TestRequestDataDto_Deserialization()
    {
        var json = data.ResponseJson["request"]?["manual"]?.ToString();
        json.Should().NotBeNull();

        var requestData = JsonSerializer.Deserialize<RequestDataDto>(json);

        requestData.Should().NotBeNull();
        requestData.Fn.Should().Be("7380440800939112");
        requestData.Fd.Should().Be("30641");
        requestData.Fp.Should().Be("1310356009");
        requestData.Timestamp.Should().Be("09.04.2025 20:07");
        requestData.OperationType.Should().Be(1);
        requestData.Sum.Should().Be("579.44");
    }

    [Fact]
    public void TestRequestInfoDto_Deserialization()
    {
        var json = data.ResponseJson["request"]?.ToString();
        json.Should().NotBeNull();

        var requestInfo = JsonSerializer.Deserialize<RequestInfoDto>(json);
        requestInfo.Should().NotBeNull();

        requestInfo.QrUrl.Should().BeEmpty();
        requestInfo.QrFileInfo.Should().BeEmpty();
        requestInfo.QrRaw.Should().BeEmpty();
        requestInfo.Input.Should().NotBeNull();
    }

    [Fact]
    public void TestItemDto_Deserialization()
    {
        var json = data.ResponseJson["data"]?["json"]?["items"]?[0]?.ToString();
        json.Should().NotBeNull();

        var item = JsonSerializer.Deserialize<ItemDto>(json);
        item.Should().NotBeNull();

        item.Quantity.Should().Be(1);
        item.Price.Should().Be(8763);
        item.Sum.Should().Be(8763);
        item.Nds.Should().Be(2);
        item.NdsSum.Should().Be(797);
        item.Name.Should().Be("НАП РАСТ HI С МИН");

        json = data.ResponseJson["data"]?["json"]?["items"]?.ToString();
        json.Should().NotBeNull();

        var items = JsonSerializer.Deserialize<List<ItemDto>>(json);
        items.Should().NotBeNull();
        items.Should().HaveCount(5);
    }

    [Fact]
    public void TestCheckInfoJsonDto_Deserialization()
    {
        var json = data.ResponseJson["data"]?["json"]?.ToString();
        json.Should().NotBeNull();

        var checkInfoJson = JsonSerializer.Deserialize<CheckInfoDataJsonDto>(json);
        checkInfoJson.Should().NotBeNull();

        checkInfoJson.User.Should().Be("ООО \"АШАН\"");
        checkInfoJson.RetailPlace.Should().Be("Гипермаркет АШАН");
        checkInfoJson.RetailPlaceAddress.Should().Be("109052, г.Москва, Рязанский пр-кт, 2, к.2");
        checkInfoJson.Items.Should().HaveCount(5);
        checkInfoJson.Nds10.Should().Be(2302);
        checkInfoJson.Nds18.Should().Be(5439);
        checkInfoJson.FnsUrl.Should().Be("www.nalog.gov.ru");
        checkInfoJson.Timestamp.Should().Be(new DateTime(2025, 4, 9, 20, 7, 0));
        checkInfoJson.Sum.Should().Be(57944);
    }

    [Fact]
    public void TestCheckInfoDto_Deserialization()
    {
        var json = data.ResponseJson["data"]?.ToString();
        var html = data.ResponseJson["data"]?["html"]?.ToString();
        json.Should().NotBeNull();

        var checkInfo = JsonSerializer.Deserialize<CheckInfoDataDto>(json);
        checkInfo.Should().NotBeNull();

        checkInfo.JsonData.Should().NotBeNull();
        checkInfo.JsonData.Should().BeOfType<CheckInfoDataJsonDto>();
        checkInfo.HtmlData.Should().Be(html);
    }

    [Fact]
    public void TestGetCheckInfoResponseDto_Deserialization()
    {
        var json = data.ResponseJson.ToString();
        json.Should().NotBeNull();

        var getCheckInfoResponse = JsonSerializer.Deserialize<GetCheckInfoResponseDto>(json);
        getCheckInfoResponse.Should().NotBeNull();

        getCheckInfoResponse.StatusCode.Should().Be(ScanResultEnum.Success);
        getCheckInfoResponse.IsFirst.Should().BeFalse();
        getCheckInfoResponse.Data.Should().NotBeNull();
        getCheckInfoResponse.Data.Should().BeOfType<CheckInfoDataDto>();
        getCheckInfoResponse.RequestInfo.Should().NotBeNull();
        getCheckInfoResponse.RequestInfo.Should().BeOfType<RequestInfoDto>();
    }

    #endregion

    #region Request Serialization

    [Fact]
    public void TestGetCheckInfoDto_Serialization()
    {
        var json = JsonSerializer.Serialize(data.RequestDto, data.Options);
        output.WriteLine($"json: {json}");

        const string finalJson = """
                                 {
                                   "token": "testToken",
                                   "fn": 7380440800939112,
                                   "fd": 30641,
                                   "fp": 1310356009,
                                   "t": "09.04.2025 20:07",
                                   "n": 1,
                                   "s": 579.44,
                                   "qr": 0
                                 }
                                 """;
        json.Should().Be(finalJson);
    }

    #endregion
}