using System.Collections.Generic;

namespace Meetings.Logic.Observer
{
    /// <summary>
    /// Интерфейс наблюдателя объектов.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObserver<T> where T : class
    {
        /// <summary>
        /// Осуществляет подписку объектов на события.
        /// </summary>
        /// <param name="items">Список элементов.</param>
        void Update(IEnumerable<T> items);
    }
}