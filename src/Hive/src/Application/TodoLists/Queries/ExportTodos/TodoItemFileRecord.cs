using Hive.Application.Common.Mappings;
using Hive.Domain.Entities;

namespace Hive.Application.TodoLists.Queries.ExportTodos
{
    public class TodoItemRecord : IMapFrom<TodoItem>
    {
        public string Title { get; set; }

        public bool Done { get; set; }
    }
}
