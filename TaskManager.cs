using System.Diagnostics;

namespace TaskManagerApp;

public class TaskManager
{
    private readonly List<Task> tasks = new();

    public void AddTask(string title)
    {
        Trace.WriteLine("[TRACE] Начало операции AddTask");
        Trace.WriteLine($"[TRACE] Проверка введённого названия: \"{title}\"");
        title = title.Trim();

        if (!IsValidTitle(title, "add"))
        {
            Trace.WriteLine("[TRACE] Конец операции AddTask: неуспех");
            return;
        }

        Trace.WriteLine("[TRACE] Создание задачи и добавление в коллекцию");
        tasks.Add(new Task { Title = title });
        Trace.TraceInformation($"Задача \"{title}\" успешно добавлена");
        Trace.TraceInformation($"Всего задач: {tasks.Count}");
        Trace.WriteLine("[TRACE] Конец операции AddTask: успех");
    }

    public void RemoveTask(string title)
    {
        Trace.WriteLine("[TRACE] Начало операции RemoveTask");
        Trace.WriteLine($"[TRACE] Проверка введённого названия: \"{title}\"");
        title = title.Trim();

        if (!IsValidTitle(title, "remove"))
        {
            Trace.WriteLine("[TRACE] Конец операции RemoveTask: неуспех");
            return;
        }

        Trace.WriteLine($"[TRACE] Поиск задачи \"{title}\" в коллекции");
        int index = tasks.FindIndex(task =>
            string.Equals(task.Title, title, StringComparison.OrdinalIgnoreCase));

        if (index == -1)
        {
            Trace.TraceError($"Задача \"{title}\" не найдена для удаления");
            Trace.TraceInformation($"Всего задач: {tasks.Count}");
            Trace.WriteLine("[TRACE] Конец операции RemoveTask: неуспех");
            return;
        }

        string removedTitle = tasks[index].Title;
        Trace.WriteLine($"[TRACE] Удаление задачи с номером {index + 1}");
        tasks.RemoveAt(index);
        Trace.TraceInformation($"Задача \"{removedTitle}\" успешно удалена");
        Trace.TraceInformation($"Всего задач: {tasks.Count}");
        Trace.WriteLine("[TRACE] Конец операции RemoveTask: успех");
    }

    public void ListTasks()
    {
        Trace.WriteLine("[TRACE] Начало операции ListTasks");
        Trace.WriteLine("[TRACE] Проверка количества задач в коллекции");

        if (tasks.Count == 0)
        {
            Trace.TraceInformation("Список задач пуст");
        }
        else
        {
            Trace.WriteLine("[TRACE] Вывод задач с нумерацией");
            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tasks[i].Title}");
            }
        }

        Trace.TraceInformation($"Всего задач: {tasks.Count}");
        Trace.WriteLine("[TRACE] Конец операции ListTasks: успех");
    }

    private static bool IsValidTitle(string title, string command)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            Trace.TraceWarning($"Пользователь ввёл пустое название. Операция {command} не выполнена");
            return false;
        }

        if (title.Length > 100 || title.Any(char.IsControl))
        {
            Trace.TraceWarning($"Невалидное название: допустимо до 100 символов без управляющих символов. Операция {command} не выполнена");
            return false;
        }

        return true;
    }
}
