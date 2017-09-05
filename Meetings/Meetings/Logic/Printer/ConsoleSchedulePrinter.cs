using System;
using System.Collections.Generic;
using Meetings.Data.Models;

namespace Meetings.Logic.Printer
{
    /// <summary>
    /// Вывод элементов в консоль.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class ConsoleSchedulePrinter : IConsoleSchedulePrinter<Meeting>
    {
        /// <summary>
        /// Записывает текстовое представление заданных встреч в консоль, соответствующих указанной дате.
        /// </summary>
        /// <param name="meetings">Список dcnhtx для вывода в консоль.</param>
        /// <param name="day">Дата.</param>
        public void Print(IEnumerable<Meeting> meetings, DateTime day)
        {
            bool isFound = false;
            foreach (Meeting meeting in meetings)
            {
                if (meeting.BeginDateTime.ToShortDateString() == day.ToShortDateString())
                {
                    isFound = true;
                }
            }
            if (isFound)
            {
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine("Расписание встреч на " + day.ToLongDateString());
                foreach (Meeting meeting in meetings)
                {
                    if (meeting.BeginDateTime.ToShortDateString() == day.ToShortDateString())
                    {
                        Console.WriteLine("\r\nВстреча № " + meeting.Id.ToString() + " назначена на \t" + meeting.BeginDateTime.ToString());
                        Console.WriteLine("Встреча закончится \t\t" + meeting.EndDateTime.ToString());
                        if (meeting.NoteDateTime != null) Console.WriteLine("Уведомление о встрече \t\t" + meeting.NoteDateTime.ToString());
                        else Console.WriteLine("Уведомления не назначено");
                    }
                }
                Console.WriteLine("---------------------------------------------------");
            }
            else
            {
                Console.WriteLine(day.ToLongDateString() + " не запланировано встреч");
            }
        }
    }
}
