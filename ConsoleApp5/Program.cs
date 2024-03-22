using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class KlineData
{
    public long OpenTime { get; set; }
    public string OpenPrice { get; set; }
    public string HighPrice { get; set; }
    public string LowPrice { get; set; }
    public string ClosePrice { get; set; }
    public string Volume { get; set; }
    public long CloseTime { get; set; }
    public string QuoteAssetVolume { get; set; }
    public int NumberOfTrades { get; set; }
    public string TakerBuyBaseAssetVolume { get; set; }
    public string TakerBuyQuoteAssetVolume { get; set; }
    public string UnusedField { get; set; }
}

public class Program
{
    public static async Task Main()
    {
        string url = "https://api.binance.com/api/v3/klines";
        string symbol = "btcusdt";
        string interval = "1d";

        string fullUrl = url + $"?symbol={symbol}&interval={interval}";

        HttpClient httpClient = new HttpClient();
        var response = await httpClient.GetAsync(fullUrl);

        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();

            var klineDataList = JsonSerializer.Deserialize<List<List<object>>>(responseData);

            var klineDataObjects = new List<KlineData>();

            foreach (var klineData in klineDataList)
            {
                var klineDataObject = new KlineData
                {
                    OpenTime = Convert.ToInt64(klineData[0]),
                    OpenPrice = klineData[1].ToString(),
                    HighPrice = klineData[2].ToString(),
                    LowPrice = klineData[3].ToString(),
                    ClosePrice = klineData[4].ToString(),
                    Volume = klineData[5].ToString(),
                    CloseTime = Convert.ToInt64(klineData[6]),
                    QuoteAssetVolume = klineData[7].ToString(),
                    NumberOfTrades = Convert.ToInt32(klineData[8]),
                    TakerBuyBaseAssetVolume = klineData[9].ToString(),
                    TakerBuyQuoteAssetVolume = klineData[10].ToString(),
                    UnusedField = klineData[11].ToString()
                };

                klineDataObjects.Add(klineDataObject);
            }

            string json = JsonSerializer.Serialize(klineDataObjects);

            await File.WriteAllTextAsync("kline_data.json", json);

            Console.WriteLine("Data has been serialized and saved to kline_data.json");
        }
        else
        {
            Console.WriteLine("Error occurred while fetching data from the API.");
        }
    }
}
