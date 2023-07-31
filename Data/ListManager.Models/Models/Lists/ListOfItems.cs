using System.Text.Json.Serialization;
using System.Text.Json;

namespace ListManager.Models.Lists;

public interface IListOfItems
{
	string Id { get; }
	string ListId { get; }
	List<IListItem> ListItems { get; }
	IListItem CreateListItem(string ItemId, string Description);
	Task MoveUp(int position);
	Task MoveDown(int position);
}

public class ListOfItems(string id,
												 string listId,
												 List<IListItem>? listItems = default) : IListOfItems
{
	public string Id { get; } = id;
	public string ListId { get; } = listId;
	[JsonConverter(typeof(ListConverter<ListItem>))]
	public List<IListItem> ListItems { get; } = new List<IListItem>(listItems?.Cast<IListItem>() ?? new List<ListItem>());
	public IListItem CreateListItem(string ItemId, string Description)
	{
		ListItem listItem = new ListItem(ItemId, Description);
		ListItems.Add(listItem);
		return listItem;
	}
	public Task MoveUp(int Position)
	{
		if (Position == 1) return Task.CompletedTask;
		var item = ListItems.FirstOrDefault(l => l.ListPosition == Position);
		if (item is null) return Task.CompletedTask;
		var item2 = ListItems.FirstOrDefault(l => l.ListPosition == Position - 1);
		item.ListPosition = Position - 1;
		if (item2 is null) return Task.CompletedTask;
		item2.ListPosition = Position;
		return Task.CompletedTask;
	}
	public Task MoveDown(int Position)
	{
		if (Position == ListItems.Count) return Task.CompletedTask;
		var item = ListItems.FirstOrDefault(l => l.ListPosition == Position);
		if (item is null) return Task.CompletedTask;
		var item2 = ListItems.FirstOrDefault(l => l.ListPosition == Position + 1);
		item.ListPosition = Position + 1;
		if (item2 is null) return Task.CompletedTask;
		item2.ListPosition = Position;
		return Task.CompletedTask;
	}
}

public class ListConverter<T> : JsonConverter<List<IListItem>>
{
	public override List<IListItem> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		List<T>? listItems = JsonSerializer.Deserialize<List<T>>(ref reader, options);
		return new List<IListItem>(listItems.Cast<IListItem>());
	}

	public override void Write(Utf8JsonWriter writer, List<IListItem> value, JsonSerializerOptions options)
	{
		JsonSerializer.Serialize(writer, new List<T>(value.Cast<T>()), options);
	}
}