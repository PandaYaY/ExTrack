using System.Text.Json;
using Newtonsoft.Json.Linq;
using ProverkaCheka.Dto;

namespace ProverkaCheka.Test;

[TestFixture]
public class SerializationTests
{
    private          JObject                _responseJson;
    private          GetCheckInfoRequestDto _requestDto;
    private readonly JsonSerializerOptions  _options = new() { WriteIndented = true };

    [OneTimeSetUp]
    public void Setup()
    {
        var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "Mock", "Response.json");
        TestContext.Out.WriteLine($"Path: {path}");
        var responseJson = File.ReadAllText(path);
        _responseJson = JObject.Parse(responseJson);

        _requestDto = new GetCheckInfoRequestDto("testToken", 7380440800939112, 30641, 1310356009, "09.04.2025 20:07",
                                                 OperationType.Prihod, 579.44, false);
    }

#region Response Deserialization

    [Test]
    public void TestRequestDataDtoDeserialization()
    {
        var json = _responseJson["request"]?["manual"]?.ToString();
        Assert.That(json, Is.Not.Null, "JSON для RequestDataDto не должен быть null");

        var requestData = JsonSerializer.Deserialize<RequestDataDto>(json);

        Assert.That(requestData, Is.Not.Null, "Объект RequestDataDto не должен быть null");
        Assert.Multiple(() =>
        {
            Assert.That(requestData.Fn, Is.EqualTo("7380440800939112"), "Значение Fn не соответствует ожидаемому");
            Assert.That(requestData.Fd, Is.EqualTo("30641"), "Значение Fd не соответствует ожидаемому");
            Assert.That(requestData.Fp, Is.EqualTo("1310356009"), "Значение Fp не соответствует ожидаемому");
            Assert.That(requestData.Timestamp, Is.EqualTo("09.04.2025 20:07"),
                        "Значение Timestamp не соответствует ожидаемому");
            Assert.That(requestData.OperationType, Is.EqualTo("1"),
                        "Значение OperationType не соответствует ожидаемому");
            Assert.That(requestData.Sum, Is.EqualTo("579.44"), "Значение Sum не соответствует ожидаемому");
        });
    }

    [Test]
    public void TestRequestInfoDtoDeserialization()
    {
        var json = _responseJson["request"]?.ToString();
        Assert.That(json, Is.Not.Null, "JSON для RequestInfoDto не должен быть null");

        var requestInfo = JsonSerializer.Deserialize<RequestInfoDto>(json);
        Assert.That(requestInfo, Is.Not.Null, "Объект RequestInfoDto не должен быть null");

        Assert.Multiple(() =>
        {
            Assert.That(requestInfo.QrUrl, Is.EqualTo(""), "Значение QrUrl не соответствует ожидаемому");
            Assert.That(requestInfo.QrFileInfo, Is.EqualTo(""), "Значение QrFileInfo не соответствует ожидаемому");
            Assert.That(requestInfo.QrRaw, Is.EqualTo(""), "Значение QrRaw не соответствует ожидаемому");
            Assert.That(requestInfo.Input, Is.Not.Null, "Объект Input не должен быть null");
        });
    }

    [Test]
    public void TestItemDtoDeserialization()
    {
        var json = _responseJson["data"]?["json"]?["items"]?[0]?.ToString();
        Assert.That(json, Is.Not.Null, "JSON для ItemDto не должен быть null");

        var item = JsonSerializer.Deserialize<ItemDto>(json);
        Assert.That(item, Is.Not.Null, "Объект ItemDto не должен быть null");

        Assert.Multiple(() =>
        {
            Assert.That(item.Quantity, Is.EqualTo(1), "Значение Quantity не соответствует ожидаемому");
            Assert.That(item.Price, Is.EqualTo(8763), "Значение Price не соответствует ожидаемому");
            Assert.That(item.Sum, Is.EqualTo(8763), "Значение Sum не соответствует ожидаемому");
            Assert.That(item.Nds, Is.EqualTo(2), "Значение Nds не соответствует ожидаемому");
            Assert.That(item.NdsSum, Is.EqualTo(797), "Значение NdsSum не соответствует ожидаемому");
            Assert.That(item.Name, Is.EqualTo("НАП РАСТ HI С МИН"), "Значение Name не соответствует ожидаемому");
        });

        json = _responseJson["data"]?["json"]?["items"]?.ToString();
        Assert.That(json, Is.Not.Null, "JSON для List<ItemDto> не должен быть null");

        var items = JsonSerializer.Deserialize<List<ItemDto>>(json);
        Assert.Multiple(() =>
        {
            Assert.That(item, Is.Not.Null, "Объект List<ItemDto> не должен быть null");
            Assert.That(items!, Has.Count.EqualTo(5), "Количество элементов в списке должно быть равно 5");
        });
    }

    [Test]
    public void TestCheckInfoJsonDtoDeserialization()
    {
        var json = _responseJson["data"]?["json"]?.ToString();
        Assert.That(json, Is.Not.Null, "JSON для CheckInfoDataJsonDto не должен быть null");

        var checkInfoJson = JsonSerializer.Deserialize<CheckInfoDataJsonDto>(json);
        Assert.That(checkInfoJson, Is.Not.Null, "Объект CheckInfoDataJsonDto не должен быть null");

        Assert.Multiple(() =>
        {
            Assert.That(checkInfoJson.User, Is.EqualTo("ООО \"АШАН\""), "Значение User не соответствует ожидаемому");
            Assert.That(checkInfoJson.RetailPlace, Is.EqualTo("Гипермаркет АШАН"),
                        "Значение RetailPlace не соответствует ожидаемому");
            Assert.That(checkInfoJson.RetailPlaceAddress, Is.EqualTo("109052, г.Москва, Рязанский пр-кт, 2, к.2"),
                        "Значение RetailPlaceAddress не соответствует ожидаемому");
            Assert.That(checkInfoJson.Items, Has.Count.EqualTo(5), "Значение Items не соответствует ожидаемому");
            Assert.That(checkInfoJson.Nds10, Is.EqualTo(2302), "Значение Nds10 не соответствует ожидаемому");
            Assert.That(checkInfoJson.Nds18, Is.EqualTo(5439), "Значение Nds18 не соответствует ожидаемому");
            Assert.That(checkInfoJson.FnsUrl, Is.EqualTo("www.nalog.gov.ru"),
                        "Значение FnsUrl не соответствует ожидаемому");
            Assert.That(checkInfoJson.Timestamp, Is.EqualTo(new DateTime(2025, 4, 9, 20, 7, 0)),
                        "Значение Timestamp не соответствует ожидаемому");
            Assert.That(checkInfoJson.Sum, Is.EqualTo(57944), "Значение Sum не соответствует ожидаемому");
        });
    }

    [Test]
    public void TestCheckInfoDtoDeserialization()
    {
        var json = _responseJson["data"]?.ToString();
        var html = _responseJson["data"]?["html"]?.ToString();
        Assert.That(json, Is.Not.Null, "JSON для CheckInfoDataDto не должен быть null");

        var checkInfo = JsonSerializer.Deserialize<CheckInfoDataDto>(json);
        Assert.That(checkInfo, Is.Not.Null, "Объект CheckInfoDataDto не должен быть null");

        Assert.Multiple(() =>
        {
            Assert.That(checkInfo.JsonData, Is.Not.Null, "Значение JsonData не должно быть null");
            Assert.That(checkInfo.JsonData.GetType(), Is.EqualTo(typeof(CheckInfoDataJsonDto)),
                        "Значение JsonData должно быть CheckInfoDataJsonDto");
            Assert.That(checkInfo.HtmlData, Is.EqualTo(html), "Значение RetailPlace не соответствует ожидаемому");
        });
    }

    [Test]
    public void TestGetCheckInfoResponseDtoDeserialization()
    {
        var json = _responseJson.ToString();
        Assert.That(json, Is.Not.Null, "JSON для GetCheckInfoResponseDto не должен быть null");

        var getCheckInfoResponse = JsonSerializer.Deserialize<GetCheckInfoResponseDto>(json);
        Assert.That(getCheckInfoResponse, Is.Not.Null, "Объект GetCheckInfoResponseDto не должен быть null");

        Assert.Multiple(() =>
        {
            Assert.That(getCheckInfoResponse.StatusCode, Is.EqualTo(ScanResultEnum.Success),
                        "Значение StatusCode не соответствует ожидаемому");
            Assert.That(getCheckInfoResponse.IsFirst, Is.False, "Значение IsFirst не соответствует ожидаемому");
            Assert.That(getCheckInfoResponse.Data, Is.Not.Null, "Значение Data не должно быть null");
            Assert.That(getCheckInfoResponse.Data.GetType(), Is.EqualTo(typeof(CheckInfoDataDto)),
                        "Значение Data должно быть CheckInfoDataDto");
            Assert.That(getCheckInfoResponse.RequestInfo, Is.Not.Null, "Значение RequestInfo не должно быть null");
            Assert.That(getCheckInfoResponse.RequestInfo.GetType(), Is.EqualTo(typeof(RequestInfoDto)),
                        "Значение RequestInfo должно быть RequestInfoDto");
        });
    }

#endregion

#region Request Serialization

    [Test]
    public void TestGetCheckInfoDtoSerialization()
    {
        var json = JsonSerializer.Serialize(_requestDto, _options);
        TestContext.Out.WriteLine($"json: {json}");

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
        Assert.That(json, Is.EqualTo(finalJson));
    }

#endregion
}
