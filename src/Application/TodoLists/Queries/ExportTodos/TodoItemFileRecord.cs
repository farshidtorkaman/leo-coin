using Crypto.Application.Common.Mappings;
using Crypto.Domain.Entities;

namespace Crypto.Application.TodoLists.Queries.ExportTodos
{
    public class TodoItemRecord : IMapFrom<TodoItem>
    {
        public string Title { get; set; }

        public bool Done { get; set; }
    }
}
