using System.Collections;
using AutoMapper;
using HabiticaTodosSorter.Services;
using HabiticaTodosSorter.Models.Tags;
using HabiticaTodosSorter.Models.Todos;

namespace HabiticaTodosSorter.Extensions;

public static class TodosExtensions
{
    public static IList<Todo> SetTaskPosition(this IList<Todo> todos)
    {
        for (var i = 0; i < todos.Count; i++)
        {
            todos[i].TaskPosition = i;
        }

        return todos;
    }

    public static IList<Todo> AssignTagNames(this IList<Todo> todos, IEnumerable<Tag> tagsWithNames)
    {
        // todo: caching np. Redis
        
        foreach (var todo in todos)
        {
            foreach (var todoTag in todo.Tags)
            {
                todoTag.Name = tagsWithNames.Where(tag => tag.Id == todoTag.Id).Select(t => t.Name).FirstOrDefault();
            }
        }

        return todos;
    }
}