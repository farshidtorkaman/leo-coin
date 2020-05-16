using Crypto.Application.TodoLists.Queries.ExportTodos;
using System.Collections.Generic;

namespace Crypto.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
    }
}
