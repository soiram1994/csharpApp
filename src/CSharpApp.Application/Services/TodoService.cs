namespace CSharpApp.Application.Services;

public class TodoService(
    HttpClient client) : ITodoService
{
    public async Task<TodoRecord?> GetTodoById(int id)
    {
        var response = await client.GetFromJsonAsync<TodoRecord>($"todos/{id}");

        return response;
    }

    public async Task<ReadOnlyCollection<TodoRecord>> GetAllTodos()
    {
        var response = await client.GetFromJsonAsync<List<TodoRecord>>($"todos");

        return response!.AsReadOnly();
    }
}