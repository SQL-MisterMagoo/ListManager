using System;

namespace ListManager.Models.Lists;

public readonly record struct List(
    string Id,
    string ListId,
    ListItem[] ListItems
    );
