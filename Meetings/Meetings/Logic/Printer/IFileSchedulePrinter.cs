using System;
using System.Collections.Generic;

namespace Meetings.Logic.Printer
{
    /// <summary>
    /// Запись элементов в файл.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFileSchedulePrinter<T> where T : class
    {
        /// <summary>
        /// Записывает текстовое представление элементов, соответствующих указанной дате, в файл по заданнному пути.
        /// </summary>
        /// <param name="items">Список элементов.</param>
        /// <param name="day">Дата.</param>
        /// <param name="path">Путь сохранения файла.</param>
        void Print(IEnumerable<T> items, DateTime day, string path);
    }
}
