using System.Collections.Generic;

public static class ListExtensions
{
    public static T Draw<T>(this List<T> list)
    {
        if (list.Count == 0) return default;
        int randomIndex = UnityEngine.Random.Range(0, list.Count);
        T t = list[randomIndex];
        list.Remove(t);
        return t;
    }
}
