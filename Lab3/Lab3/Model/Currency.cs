using Newtonsoft.Json;

public class CurrencyConversionResponse
{
    [JsonProperty("valid")]
    public bool Valid { get; set; }

    [JsonProperty("from-type")]
    public string FromType { get; set; }

    [JsonProperty("from-value")]
    public int FromValue { get; set; }

    [JsonProperty("result")]
    public double Result { get; set; }

    [JsonProperty("result-float")]
    public double ResultFloat { get; set; }

    [JsonProperty("to-type")]
    public string ToType { get; set; }
}