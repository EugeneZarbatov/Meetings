using System;
using System.Threading;
using Meetings.Data.Models;
using Meetings.Logic.Printer;
using Meetings.Logic.Shedule;
using Meetings.Logic.Observer;
using System.IO;

namespace Meetings.View
{
    /// <summary>
    /// Интерфейс приложения.
    /// </summary>
    public class Interface
    {
        /// <summary>
        /// Ссылка на объект класса Interface (singleton).
        /// </summary>
        private static Interface _instance;
        /// <summary>
        /// Расписание.
        /// </summary>
        private ISchedule _schedule = new Schedule();
        /// <summary>
        /// Наблюдатель встреч. В качестве делегата на обработку событий передается консольный вывод.
        /// </summary>
        private Logic.Observer.IObserver<Meeting> _observer = new MeetingObserver(message => Console.WriteLine(message));
        /// <summary>
        /// Принтер для вывода расписания в консоль.
        /// </summary>
        private IConsoleSchedulePrinter<Meeting> _consolePrinter = new ConsoleSchedulePrinter();
        /// <summary>
        /// Принтер для записи расписания в файл.
        /// </summary>
        private IFileSchedulePrinter<Meeting> _filePrinter = new FileSchedulePrinter();
        /// <summary>
        /// Конструктор (singleton).
        /// </summary>
        private Interface() { }

        /// <summary>
        /// Возвращает ссылку на объект класса Interface (singlton).
        /// </summary>
        /// <returns></returns>
        public static Interface getInstance()
        {
            if (_instance == null)
            {
                _instance = new Interface();
            }
            return _instance;
        }
        /// <summary>
        /// Наблюдает расписание с периодом 1 секунда.
        /// </summary>
        /// <param name="obj">Расписание.</param>
        private void Update(object obj)
        {
            try
            {
                (obj as Schedule).Update(_observer);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.TargetSite}: {e.Message}");
            }
        }
        /// <summary>
        /// Инициализирует меню консоли.
        /// </summary>
        public void Init()
        {
            Console.Title = "Meetings";
            Timer timer = new Timer(Update, _schedule, 0, 1000);
            int key = 0;
            bool isAlive = true;
            while (isAlive)
            {
                Console.WriteLine("Выберите операцию");
                Console.WriteLine("1 - Создать встречу \t\t 5 - Показать расписание на день");
                Console.WriteLine("2 - Отменить встречу \t\t 6 - Сохранить расписание на день");
                Console.WriteLine("3 - Изменить встречу \t\t 7 - Закрыть приложение");
                Console.WriteLine("4 - Добавить уведомление");
                if (int.TryParse(Console.ReadLine(), out key))
                {
                    Console.Clear();
                    switch (key)
                    {
                        case 1: Add(); break;
                        case 2: Remove(); break;
                        case 3: Edit(); break;
                        case 4: AddNotification(); break;
                        case 5: ConsolePrint(_consolePrinter); break;
                        case 6: FilePrint(_filePrinter); break;
                        case 7:
                            Console.WriteLine("Завершение работы приложения..");
                            timer.Dispose();
                            isAlive = false;
                            break;
                        default:
                            Console.WriteLine("Ошибка: неверная команда");
                            break;
                    }
                    Console.WriteLine("Для продолжения нажмите любую клавишу..");
                    Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Ошибка: неверная команда");
                    Console.WriteLine("Для продолжения нажмите любую клавишу..");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }
        /// <summary>
        /// Добавляет встречу.
        /// </summary>
        private void Add()
        {
            Console.WriteLine("Введите данные о встрече");
            Console.WriteLine("Формат ввода: DD.MM.YYYY HH:MM:SS");
            Console.Write("Начало встречи: ");
            string beginStr = Console.ReadLine();
            Console.Write("Конец встречи: ");
            string endStr = Console.ReadLine();
            Console.Write("Уведомление о встрече: ");
            string noteStr = Console.ReadLine();
            try
            {
                DateTime? noteDateTime;
                if (noteStr == "") noteDateTime = null;
                else noteDateTime = DateTime.Parse(noteStr);

                _schedule.Add(DateTime.Parse(beginStr), DateTime.Parse(endStr), noteDateTime);
                Console.WriteLine("Добавлена новая встреча!");
            }
            catch (FormatException)
            {
                Console.WriteLine("Введены некорректные данные!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// Удаляет встречу.
        /// </summary>
        private void Remove()
        {
            try
            {
                if (_schedule.Count() == 0) Console.WriteLine("Расписание пусто!");
                else
                {
                    Console.WriteLine("Введите номер встречи");
                    int id = int.Parse(Console.ReadLine());
                    _schedule.Remove(id);
                    Console.WriteLine($"Встреча № {id} удалена!");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Введены некорректные данные!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// добавляет уведомление встрече.
        /// </summary>
        private void AddNotification()
        {
            try
            {
                if (_schedule.Count() == 0) Console.WriteLine("Расписание пусто!");
                else
                {
                    Console.WriteLine("Введите данные о встрече");
                    Console.Write("Номер встречи: ");
                    int id = int.Parse(Console.ReadLine());
                    if (_schedule.Find(id) == null) Console.WriteLine($"Встреча № { id } не найдена!");
                    else
                    {
                        Console.Write("Время уведомления: ");
                        DateTime noteDateTime = DateTime.Parse(Console.ReadLine());

                        _schedule.AddNotification(id, noteDateTime);
                        Console.WriteLine($"Встрече № {id} добавлено уведомление!");
                    }
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Введены некорректные данные!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// Редактирует встречу.
        /// </summary>
        private void Edit()
        {
            try
            {
                if (_schedule.Count() == 0) Console.WriteLine("Расписание пусто!");
                else
                {
                    Console.WriteLine("Введите номер встречи");
                    int id = int.Parse(Console.ReadLine());
                    if (_schedule.Find(id) == null) Console.WriteLine($"Встреча № { id } не найдена!");
                    else
                    {
                        Console.WriteLine("Введите данные о встрече");
                        Console.WriteLine("Формат ввода: DD.MM.YYYY HH:MM:SS");
                        Console.Write("Начало встречи: ");
                        string beginStr = Console.ReadLine();
                        Console.Write("Конец встречи: ");
                        string endStr = Console.ReadLine();
                        Console.Write("Уведомление о встрече: ");
                        string noteStr = Console.ReadLine();

                        DateTime? noteDateTime;
                        if (noteStr == "") noteDateTime = null;
                        else noteDateTime = DateTime.Parse(noteStr);

                        _schedule.Edit(id, DateTime.Parse(beginStr), DateTime.Parse(endStr), noteDateTime);
                        Console.WriteLine($"Встреча № {id} изменена!");
                    }
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Введены некорректные данные!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// Консольный вывод.
        /// </summary>
        /// <param name="printer">Консольный принтер</param>
        private void ConsolePrint(IConsoleSchedulePrinter<Meeting> printer)
        {
            try
            {
                if (_schedule.Count() == 0) Console.WriteLine("Расписание пусто!");
                else
                {
                    Console.WriteLine("Введите дату");
                    DateTime day = DateTime.Parse(Console.ReadLine());
                    _schedule.ConsolePrint(printer, day);
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Введены некорректные данные!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// Запись в файл.
        /// </summary>
        /// <param name="printer">Файловый принтер.</param>
        private void FilePrint(IFileSchedulePrinter<Meeting> printer)
        {
            try
            {
                if (_schedule.Count() == 0) Console.WriteLine("Расписание пусто!");
                else
                {
                    string path;
                    Console.WriteLine("Введите дату");
                    DateTime day = DateTime.Parse(Console.ReadLine());
                    Console.WriteLine(@"Директория по умолчанию: C:\Users\user\Documents\Visual Studio 2015\" + "\r\n" + @"\Projects\ConsoleApplication\ConsoleApplication\bin\Debug\Output");
                    Console.Write("Сохранить в директорию по умолчанию? (да / нет): ");
                    string key = Console.ReadLine();
                    switch (key)
                    {
                        case "да":
                            {
                                if (!Directory.Exists(@"Output\"))
                                    Directory.CreateDirectory(@"Output\");
                                path = @"Output\" + day.ToShortDateString() + ".txt";
                                _schedule.FilePrint(printer, day, path);
                                break;
                            }
                        case "нет":
                            {
                                Console.Write("Введите путь: ");
                                path = Console.ReadLine();
                                _schedule.FilePrint(printer, day, path);
                                break;
                            }
                        default: Console.WriteLine("Введены некорректные данные!"); break;
                    }
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Введены некорректные данные!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}