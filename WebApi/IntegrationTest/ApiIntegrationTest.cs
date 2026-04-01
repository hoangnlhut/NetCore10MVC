using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NuGet.Protocol.Core.Types;
using System.Net.Http.Json;
using ToDoRepository;
using WA;
namespace IntegrationTest
{
    public class ApiIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ApiIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Test1()
        {
            //arrange
            var client = _factory.CreateClient();
            var item = CreateToDoItem();

            //act
            var response =  await client.GetAsync($"/api/todoitems/{item.Id}");
            response.EnsureSuccessStatusCode();

            //assert
            response.Should().BeNull();

            //creat new to do item and add it to the database
            var createdResponse = await client.PostAsJsonAsync($"/api/todoitems/{item.Id}", item);
            createdResponse.EnsureSuccessStatusCode();

            var addedItem = await createdResponse.Content.ReadFromJsonAsync<ToDoItem>();

            addedItem.Should().NotBeNull();
            addedItem.Id.Should().Be(item.Id);
            addedItem.Name.Should().Be(item.Name);
            addedItem.IsComplete.Should().Be(item.IsComplete);

            //get item by id and check if it is not null with act and assert
            //act
            var getResponse = await client.GetAsync($"/api/todoitems/{item.Id}");
            getResponse.EnsureSuccessStatusCode();

            var itemFromDb = await getResponse.Content.ReadFromJsonAsync<ToDoItem>();

            itemFromDb.Should().NotBeNull();
            itemFromDb.Id.Should().Be(item.Id);
            itemFromDb.Name.Should().Be(item.Name);
            itemFromDb.IsComplete.Should().Be(item.IsComplete);

            //update item and check if it is updated with act and assert
            //act
            item.Name = "Updated Name ApiIntegrationTest";
            item.IsComplete = true;

            var updateResponse = await client.PutAsJsonAsync($"/api/todoitems/{item.Id}", item);
            updateResponse.EnsureSuccessStatusCode();
        }

        private static ToDoItem CreateToDoItem()
        {
            return new ToDoItem
            {
                Id = Guid.NewGuid(),
                Name = GenerateNameOfToDoItem(),
                IsComplete = false
            };
        }

        private static string GenerateNameOfToDoItem()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = Random.Shared;
            return new string(Enumerable.Range(0, 50)
                .Select(_ => chars[random.Next(chars.Length)])
                .ToArray());
        }
    }
}