using System.Collections.Generic;

namespace Meetings.Data.Repositories
{
    /// <summary>
    /// Интерфейс репозитория.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface IDbRepository<T> where T : class
    {
        /// <summary>
        /// Извлекает все элементы репозитория.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> FindAll();

        /// <summary>
        /// Выполняет поиск элемента с идентификаторм id и возвращает первое вхождение в пределах репозитория.
        /// </summary>
        /// <param name="id">Идентификатор искомого элемента.</param>
        /// <returns></returns>
        T Find(int id);

        /// <summary>
        /// Добавляет объект в конец репозитория.
        /// </summary>
        /// <param name="item">Добавляемый элемент.</param>
        void Add(T item);

        /// <summary>
        /// Удаляет элемент с идентификаром id.
        /// </summary>
        /// <param name="id">Идентификатор удаляемого элемента.</param>
        void Remove(int id);

        /// <summary>
        /// Редактирует элемент с идентификатором id.
        /// </summary>
        /// <param name="id">Идентификатор редактируемого элемента.</param>
        /// <param name="item">Элемент, заменяющий старый элемент репозитория.</param>
        void Edit(int id, T item);

        /// <summary>
        /// Получает число элементов, содержащихся в репозитории.
        /// </summary>
        /// <returns></returns>
        int Count();
    }
}