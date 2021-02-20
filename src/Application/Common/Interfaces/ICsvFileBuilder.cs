using Hive.Application.TodoLists.Queries.ExportTodos;
using System.Collections.Generic;

namespace Hive.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
    }
}
