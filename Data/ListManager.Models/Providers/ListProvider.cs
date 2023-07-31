using ListManager.Models.Lists;

namespace ListManager.Providers;

public interface IListProvider
{
	IListOfItems CreateList(string listName);
	IEnumerable<IListOfItems> GetLists();
	void SaveLists();
}
public interface IListProviderAsync
{
	Task<IListOfItems> CreateList(string listName);
	Task<IListItem> CreateListItem(string listId, string description);
	Task<List<IListOfItems>> GetLists();
	Task MoveDown(IListOfItems list, int position);
	Task MoveUp(IListOfItems list, int position);
	Task SaveLists();
}
