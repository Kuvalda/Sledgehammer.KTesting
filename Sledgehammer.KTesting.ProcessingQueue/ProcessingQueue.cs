using System;
using System.Collections.Generic;
using System.Threading;

namespace Sledgehammer.KTesting
{
    /// <summary>
    /// Данный класс реализует очередь с ожиданием для задач многопоточной обработки поступающих в очерпедь данных.
    /// 
    /// Требования к реализации:
    /// 1. Надо сделать очередь с операциями push(T) и T pop().
    /// 2. Операции должны поддерживать обращение с разных потоков.
    /// 3. Операция push всегда вставляет и выходит.
    /// 4. Операция pop ждет пока не появится новый элемент.
    /// 5. В качестве контейнера внутри можно использовать только стандартную очередь (Queue).
    /// </summary>
    /// <typeparam name="T">Тип элементов очереди.</typeparam>
    public sealed class ProcessingQueue<T>
    {
        /// <summary>
        /// Контейнер очереди данных
        /// </summary>
        private Queue<T> _queue = new Queue<T>();

        /// <summary>
        /// Испорльзуем SemaphoreSlim для управления потоками, так как он не испльзует системных ресурсов ядра,
        /// и идеален для синхронизации потоков в одном процессе.
        /// </summary>
        private SemaphoreSlim _semaphore = new SemaphoreSlim(0);

        /// <summary>
        /// Помещает один элемент в очередь и разблокирует один ожидающий процесс.
        /// Метод потокобезопасен.
        /// </summary>
        /// <param name="item">Помещаемый в очередь элемент.</param>
        public void Push(T item)
        {
            lock (_queue)
            {
                _queue.Enqueue(item);
                _semaphore.Release();
            }
        }

        /// <summary>
        /// Извлекает из очереди очередной элемент.
        /// Если элементов нет, то поток засыпает в ожидании пояления элемента.
        /// Метод потокобезопасен.
        /// </summary>
        /// <returns>Извлеченный элемент.</returns>
        public T Pop()
        {
            _semaphore.Wait();
            lock (_queue)
            {
                if (_queue.Count > 0)
                {
                    return _queue.Dequeue();
                }
                else
                {
                    // Такая ситуация маловероятна, так как счетчик семафора синхронизирован с числом элементов очереди,
                    // но она возможна при ошибках в логике очереди, например если реализовать метод Clear и забыть очистить семафор.
                    throw new InvalidOperationException("Неожиданно очередь оказалась пуста.");
                }
            }
        }

    }
}
