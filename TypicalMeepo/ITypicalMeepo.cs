using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Meepo.Core.Configs;
using TypicalMeepo.Core.Events;

namespace TypicalMeepo
{
    public interface ITypicalMeepo : IDisposable
    {
        /// <summary>
        /// Get IDs and addresses of all connected servers.
        /// </summary>
        /// <returns></returns>
        Dictionary<Guid, TcpAddress> GetServerClientInfos();

        /// <summary>
        /// Remove client.
        /// </summary>
        /// <param name="id">Client ID</param>
        void RemoveClient(Guid id);

        /// <summary>
        /// Send data to a specific client.
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <param name="data">Data to send</param>
        /// <returns></returns>
        Task SendAsync<T>(Guid id, T data);

        /// <summary>
        /// Send data to a all clients.
        /// Including connected servers.
        /// </summary>
        /// <param name="data">Data to send</param>
        /// <returns></returns>
        Task SendAsync<T>(T data);

        /// <summary>
        /// Run Meepo server.
        /// Starts listening for new clients
        /// and connects to specified servers.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop Meepo server.
        /// </summary>
        void Stop();

        /// <summary>
        /// Subscribe to event when message of a specific type is received.
        /// </summary>
        /// <typeparam name="T">Meepo package</typeparam>
        /// <param name="action">MessageReceivedHandler delegate</param>
        void Subscribe<T>(MessageReceivedHandler<T> action);

        /// <summary>
        /// Unsubscribe from event.
        /// </summary>
        /// <typeparam name="T">Meepo package</typeparam>
        void Unsubscribe<T>();
    }
}