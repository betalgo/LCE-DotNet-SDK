using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using Bogus;
using LaserCatEyes.DataServiceSdk;
using LaserCatEyes.Domain.Models;
using LaserCatEyes.HttpClientListener;
using Microsoft.Extensions.Options;

namespace SampleDotNetCoreClientApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var laserCatEyesDataService = new LaserCatEyesDataService(
                Options.Create(new LaserCatEyesOptions("APP_KEY")),
                Options.Create(new LaserCatEyesSystemOptions()));
            var client = new HttpClient(new LaserCatEyesHttpMessageHandler(laserCatEyesDataService));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

            var testDataFaker = new Faker<TestData>()
                .RuleFor(r => r.StringData, (faker, data) => faker.Hacker.Noun())
                .RuleFor(r => r.Id, f => Guid.NewGuid())
                .RuleFor(r => r.DoubleData, faker => faker.Random.Number());

            while (true)
            {

                var fakeData=testDataFaker.Generate();
                client.GetAsync($"https://localhost:44328/WeatherForecast?" +
                                $"Id={fakeData.Id}&StringData={fakeData.StringData}&DoubleData={fakeData.DoubleData}");
                Console.Write(".");
                Thread.Sleep(1000);

                fakeData = testDataFaker.Generate();
                fakeData.InnerClassData = testDataFaker.Generate();

                
                client.PostAsync("https://localhost:44328/WeatherForecast", new StringContent(JsonSerializer.Serialize(fakeData),Encoding.UTF8, "application/json"));
                Console.Write(".");

                fakeData = testDataFaker.Generate();
                client.GetAsync($"https://localhost:5001/WeatherForecast?" +
                                $"Id={fakeData.Id}&StringData={fakeData.StringData}&DoubleData={fakeData.DoubleData}");
                Console.Write(".");
                Thread.Sleep(1000);

                fakeData = testDataFaker.Generate();
                fakeData.InnerClassData = testDataFaker.Generate();


                client.PostAsync("https://localhost:5001/WeatherForecast", new StringContent(JsonSerializer.Serialize(fakeData), Encoding.UTF8, "application/json"));
                Console.WriteLine(".");
                Thread.Sleep(1000);
            }
        }

        private class TestData
        {
            public Guid Id { get; set; }
            public string StringData { get; set; }
            public double DoubleData { get; set; }
            public TestData InnerClassData { get; set; }
        }
    }
}