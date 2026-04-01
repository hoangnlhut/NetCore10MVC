using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ToDoRepository;

namespace RepositoryTest
{
    public class SqlServerRepositoryTest
    {
        [Fact]
        public async Task TestToDoItemInSqlServerAsync()
        {
            //Arrange

            var options = new DbContextOptionsBuilder<ToDoContext>()
                .UseSqlServer("Data Source=DESKTOP-J76PCRA\\SQLEXPRESS;Initial Catalog=ToDoApp;User ID=sa;Password=123456hoang;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30")
                .Options;

            ToDoContext context = new ToDoContext(options);

            var repository = new SqlServerToDoItemRepository(context);  
            var item = CreateToDoItem();

            //get item by id and check if it is null with act and assert
            //act
            var itemFromDb = await repository.GetItemByIdAsync(item.Id);
            //assert
            itemFromDb.Should().BeNull();


            //creat new to do item and add it to the database
            var addedItem = await repository.AddToDoItemAsync(item);
            addedItem.Should().NotBeNull();
            addedItem.Id.Should().Be(item.Id);
            addedItem.Name.Should().Be(item.Name);
            addedItem.IsComplete.Should().Be(item.IsComplete);

            //get item by id and check if it is not null with act and assert
            //act
            itemFromDb = await repository.GetItemByIdAsync(item.Id);
            itemFromDb.Should().NotBeNull();
            itemFromDb.Id.Should().Be(item.Id);
            itemFromDb.Name.Should().Be(item.Name);
            itemFromDb.IsComplete.Should().Be(item.IsComplete);

            //update item and check if it is updated with act and assert
            //act
            item.Name = "Updated Name";
            item.IsComplete = true;
            await repository.UpdateToDoItem(item.Id, item);

            itemFromDb = await repository.GetItemByIdAsync(item.Id);
            itemFromDb.Should().NotBeNull();
            itemFromDb.Id.Should().Be(item.Id);
            itemFromDb.Name.Should().Be(item.Name);
            itemFromDb.IsComplete.Should().Be(item.IsComplete);


        }

        [Fact]
        public async Task TestDeleteApiAsync()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<ToDoContext>()
                .UseSqlServer("Data Source=DESKTOP-J76PCRA\\SQLEXPRESS;Initial Catalog=ToDoApp;User ID=sa;Password=123456hoang;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30")
                .Options;
            ToDoContext context = new ToDoContext(options);
            var repository = new SqlServerToDoItemRepository(context);
            var item = CreateToDoItem();
            //creat new to do item and add it to the database
            var addedItem = await repository.AddToDoItemAsync(item);
            addedItem.Should().NotBeNull();
            addedItem.Id.Should().Be(item.Id);
            addedItem.Name.Should().Be(item.Name);
            addedItem.IsComplete.Should().Be(item.IsComplete);
            //delete item and check if it is deleted with act and assert
            //act
            await repository.DeleteToDoItem(item);
            var itemFromDb = await repository.GetItemByIdAsync(item.Id);
            itemFromDb.Should().BeNull();
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