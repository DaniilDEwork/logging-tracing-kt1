using System.Diagnostics;
using System.Text;

namespace TaskManagerApp;

public class Program
{
    public static void Main()
    {
        Console.InputEncoding = new UTF8Encoding(false);
        Console.OutputEncoding = new UTF8Encoding(false);

        string logPath = Path.Combine(Directory.GetCurrentDirectory(), "logs",
            $"taskmanager-{DateTime.Now:yyyyMMdd-HHmmss-fff}.txt");
        Trace.Listeners.Clear();
        Trace.Listeners.Add(new TaskTraceListener(logPath));
        Trace.AutoFlush = true;

        try
        {
            Trace.TraceInformation("Программа TaskManager запущена");
            Console.WriteLine($"Файл логов: {logPath}");
            ShowHelp();

            var taskManager = new TaskManager();
            bool isRunning = true;

            while (isRunning)
            {
                Console.Write("\nКоманда > ");
                string? command = Console.ReadLine();

                if (command == null)
                {
                    Trace.TraceInformation("Поток ввода завершён");
                    break;
                }

                command = command.Trim().ToLowerInvariant();

                switch (command)
                {
                    case "add":
                    case "remove":
                        Console.Write(command == "add"
                            ? "Название новой задачи: "
                            : "Название задачи для удаления: ");
                        string? title = Console.ReadLine();

                        if (title == null)
                        {
                            Trace.TraceWarning($"Операция {command} отменена: поток ввода завершён");
                            isRunning = false;
                            break;
                        }

                        if (command == "add")
                        {
                            taskManager.AddTask(title);
                        }
                        else
                        {
                            taskManager.RemoveTask(title);
                        }

                        break;
                    case "list":
                        taskManager.ListTasks();
                        break;
                    case "help":
                        ShowHelp();
                        break;
                    case "exit":
                        isRunning = false;
                        break;
                    default:
                        Trace.TraceWarning($"Неизвестная команда \"{command}\". Введите help для справки");
                        break;
                }
            }
        }
        finally
        {
            Trace.TraceInformation("Завершение работы программы");
            Trace.Close();
        }
    }

    private static void ShowHelp()
    {
        Console.WriteLine("add    - добавить задачу");
        Console.WriteLine("remove - удалить задачу по названию");
        Console.WriteLine("list   - вывести список задач");
        Console.WriteLine("help   - показать команды");
        Console.WriteLine("exit   - завершить работу");
        Console.WriteLine("Название: от 1 до 100 символов без управляющих символов");
    }
}
