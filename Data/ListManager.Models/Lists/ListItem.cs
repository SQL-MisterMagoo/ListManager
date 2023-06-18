using System;

namespace ListManager.Models.Lists;

public readonly record struct ListItem(
    string ItemId,
    string Description,
    DateTimeOffset ActionedDate
    );