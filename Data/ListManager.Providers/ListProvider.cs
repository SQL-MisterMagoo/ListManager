using System.Collections;

namespace ListManager.Providers;

public abstract class ListProvider<T> where T : ListManager.Models.List
{
    public abstract IEnumerable<T> MyProperty { get; set; }
}