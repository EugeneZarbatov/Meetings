using System;
using System.Collections.Generic;

namespace Meetings.Logic.Printer
{
    /// <summary>
    /// Вывод элементов в консоль.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IConsoleSchedulePrinter<T> where T : class
    {
        /// <summary>
        /// Записывает текстовое представление заданных элементов в консоль, соответствующих указанной дате.
        /// </summary>
        /// <param name="items">Список элементов для вывода в консоль.</param>
        /// <param name="day">Дата.</param>
        void Print(IEnumerable<T> items, DateTime day);
    }
}
