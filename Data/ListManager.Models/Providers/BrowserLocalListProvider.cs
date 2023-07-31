using System.Text.Json;
using ListManager.Models.Lists;
using Microsoft.JSInterop;

namespace ListManager.Providers;

public sealed class BrowserLocalListProvider : IListProviderAsync
{
	private readonly IJSRuntime JSRuntime;
	private List<ListOfItems> lists;
	public BrowserLocalListProvider(IJSRuntime runtime)
	{
		this.JSRuntime = runtime;
		this.lists = new List<ListOfItems>();
	}

	public async Task<IListOfItems> CreateList(string listName)
	{
		var listOfItems = new ListOfItems(Guid.NewGuid().ToString(), listName);
		lists.Add(listOfItems);
		await SaveLists();
		IListOfItems result = listOfItems;
		return result;
	}
	public async Task<IListItem> CreateListItem(string listId, string description)
	{
		var list = lists.First(l => l.ListId == listId);
		IListItem result = list.CreateListItem(Guid.NewGuid().ToString("N"), description);
		await SaveLists();
		return result;
	}

	public async Task<List<IListOfItems>> GetLists()
	{
		string? listString = default;
		try
		{
			if (JSRuntime is IJSInProcessRuntime ipjs)
			{
				listString = ipjs.Invoke<string>("localStorage.getItem", "Lists");
			}
			else
			{
				listString = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "Lists");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.GetBaseException().Message);
		}
		try
		{
			if (!string.IsNullOrWhiteSpace(listString))
			{
				await Console.Out.WriteLineAsync(listString);
				var explicitLists = JsonSerializer.Deserialize<List<ListOfItems>>(listString) ?? new();
				lists = explicitLists;
			}
			else
			{
				lists = new List<ListOfItems>();
			}
		}
		catch (JsonException)
		{
			// reset the list in localstorage
		}
		return new List<IListOfItems>(lists.Cast<IListOfItems>());
	}
	public async Task SaveLists()
	{
		if (lists is null)
		{
			return;
		}

		var json = JsonSerializer.Serialize(lists);
		await Console.Out.WriteLineAsync(json);
		if (JSRuntime is IJSInProcessRuntime ipjs)
		{
			ipjs.InvokeVoid("localStorage.setItem", "Lists", json);
		}
		else
		{
			await JSRuntime.InvokeVoidAsync("localStorage.setItem", "Lists", json);
		}
	}
	public async Task MoveUp(IListOfItems list, int position)
	{
		list?.MoveUp(position);
		await SaveLists();
	}
	public async Task MoveDown(IListOfItems list, int position)
	{
		list?.MoveDown(position);
		await SaveLists();
	}
}