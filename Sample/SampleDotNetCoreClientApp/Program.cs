using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using LaserCatEyes.DataServiceSdk;
using LaserCatEyes.Domain.Models;
using LaserCatEyes.HttpClientListener;
using Microsoft.Extensions.DependencyInjection;

namespace SampleDotNetCoreClientApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            const string baseDomain = "https://localhost:44328";
       

            var serviceCollection = new ServiceCollection().AddLogging();
            serviceCollection.AddHttpClient<ISimpleClass, SimpleClass>();
            serviceCollection.AddLaserCatEyesHttpClientListener("cd579d47-eafb-44df-bfc7-ad1f4dd013d5");

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var simpleClass = serviceProvider.GetRequiredService<ISimpleClass>();
            var client = serviceProvider.GetRequiredService<HttpClient>();
            await simpleClass.GetData();

            var testDataFaker = new Faker<TestData>()
                .RuleFor(r => r.StringData, (faker, data) => faker.Hacker.Noun())
                .RuleFor(r => r.Id, f => Guid.NewGuid())
                .RuleFor(r => r.DoubleData, faker => faker.Random.Number());

            while (true)
            {
                var fakeData = testDataFaker.Generate();
                await client.GetAsync($"{baseDomain}/WeatherForecast?" +
                                      $"Id={fakeData.Id}&StringData={fakeData.StringData}&DoubleData={fakeData.DoubleData}");
                Console.Write(".");
                Thread.Sleep(1000);

                fakeData = testDataFaker.Generate();
                fakeData.InnerClassData = testDataFaker.Generate();


                await client.PostAsync($"{baseDomain}/WeatherForecast", new StringContent(JsonSerializer.Serialize(fakeData), Encoding.UTF8, "application/json"));
                Console.Write(".");

                fakeData = testDataFaker.Generate();
                await client.GetAsync($"{baseDomain}/WeatherForecast?" +
                                      $"Id={fakeData.Id}&StringData={fakeData.StringData}&DoubleData={fakeData.DoubleData}");
                Console.Write(".");
                Thread.Sleep(1000);

                fakeData = testDataFaker.Generate();
                fakeData.InnerClassData = testDataFaker.Generate();


                await client.PostAsync($"{baseDomain}/WeatherForecast", new StringContent(JsonSerializer.Serialize(fakeData), Encoding.UTF8, "application/json"));
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

        public class SimpleClass : ISimpleClass
        {
            private readonly HttpClient _httpClient;

            public SimpleClass(HttpClient httpClient)
            {
                _httpClient = httpClient;
            }

            public async Task GetData()
            {
                const string baseDomain = "https://localhost:44328";

                var testDataFaker = new Faker<TestData>()
                    .RuleFor(r => r.StringData, (faker, data) => faker.Hacker.Noun())
                    .RuleFor(r => r.Id, f => Guid.NewGuid())
                    .RuleFor(r => r.DoubleData, faker => faker.Random.Number());

                var fakeData = testDataFaker.Generate();
                await _httpClient.GetAsync($"{baseDomain}/WeatherForecast?" +
                                           $"Id={fakeData.Id}&StringData={fakeData.StringData}&DoubleData={fakeData.DoubleData}");
            }
        }

        internal interface ISimpleClass
        {
            Task GetData();
        }
    }
}