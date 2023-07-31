namespace ListManager.Models.Lists;

public interface IListItem
{
	string ItemId { get; }
	string Description { get; }
	List<string>? Tags { get; }
	DateTimeOffset ActionedDate { get; }
	int ListPosition { get; set; }
}

public class ListItem(string itemId,
											string description,
											DateTimeOffset actionedDate = default,
											List<string>? tags = default) : IListItem
{
	public string ItemId { get; } = itemId;
	public string Description { get; } = description;
	public DateTimeOffset ActionedDate { get; } = actionedDate;
	public List<string>? Tags { get; } = tags;
	public int ListPosition { get; set; }
}