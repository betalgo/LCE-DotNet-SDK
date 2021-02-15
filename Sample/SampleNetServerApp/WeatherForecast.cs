using System;

namespace SampleNetServerApp
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
    public class TestData
    {
        public Guid Id { get; set; }
        public string StringData { get; set; }
        public double DoubleData { get; set; }
        public TestData InnerClassData { get; set; }
    }
}
