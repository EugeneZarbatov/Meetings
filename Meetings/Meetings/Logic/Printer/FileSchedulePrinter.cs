﻿using System;
using System.Collections.Generic;
using System.IO;
using Meetings.Data.Models;

namespace Meetings.Logic.Printer
{
    /// <summary>
    /// Принтер для записи элементов в файл.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class FileSchedulePrinter : IFileSchedulePrinter<Meeting>
    {
        /// <summary>
        /// Записывает текстовое представление элементов, соответствующих указанной дате, в файл по заданнному пути.
        /// Запись осуществляется даже в том случае, когда нет элементов, соответствующих указанной дате!
        /// По умолчанию path = @"Output\" + day.ToShortDateString() + ".txt";
        /// </summary>
        /// <param name="items">Список элементов.</param>
        /// <param name="day">Дата.</param>
        /// <param name="path">Путь сохранения файла.</param>
        public void Print(IEnumerable<Meeting> meetings, DateTime day, string path)
        {
            string str;
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
                str = "Расписание встреч на " + day.ToLongDateString();
                foreach (Meeting meeting in meetings)
                {
                    if (meeting.BeginDateTime.ToShortDateString() == day.ToShortDateString())
                    {
                        str += "\r\n";
                        str += "\r\nВстреча № " + meeting.Id.ToString() + " назначена на \t" + meeting.BeginDateTime.ToString();
                        str += "\r\nВстреча закончится \t\t" + meeting.EndDateTime.ToString();
                        if (meeting.NoteDateTime != null) str += "\r\nУведомление о встрече \t\t" + meeting.NoteDateTime.ToString();
                        else str += "\r\nУведомления не назначено";
                    }
                }
            }
            else
            {
                str = day.ToLongDateString() + " не запланировано встреч";
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.Write(str);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
