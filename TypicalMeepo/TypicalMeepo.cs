using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Meepo.Core.Configs;
using Meepo.Core.Logging;
using TypicalMeepo.Core.Attributes;
using TypicalMeepo.Core.Events;
using TypicalMeepo.Core.Exceptions;
using TypicalMeepo.Core.Extensions;
using TypicalMeepo.Core.Repo;

namespace TypicalMeepo
{
    public class TypicalMeepo : ITypicalMeepo
    {
        private readonly Meepo.Meepo meepo;
        private readonly TypeRepo repo;

        private readonly Dictionary<int, MessageReceivedHandler> handlers = new Dictionary<int, MessageReceivedHandler>();

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="listenerAddress">Address you want to expose</param>
        /// <param name="assemblies">Assemblies where meepo packets are defined</param>
        public TypicalMeepo(TcpAddress listenerAddress, IEnumerable<Assembly> assemblies)
        {
            repo = new TypeRepo(assemblies);
            meepo = new Meepo.Meepo(listenerAddress);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="listenerAddress">Address you want to expose</param>
        /// <param name="logger">Custom ILogger implementation</param>
        /// <param name="assemblies">Assemblies where meepo packets are defined</param>
        public TypicalMeepo(TcpAddress listenerAddress, ILogger logger, IEnumerable<Assembly> assemblies)
        {
            repo = new TypeRepo(assemblies);
            meepo = new Meepo.Meepo(listenerAddress, logger);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="listenerAddress">Address you want to expose</param>
        /// <param name="serverAddresses">List of server addresses to connect to</param>
        /// <param name="assemblies">Assemblies where meepo packets are defined</param>
        public TypicalMeepo(TcpAddress listenerAddress, IEnumerable<TcpAddress> serverAddresses, IEnumerable<Assembly> assemblies)
        {
            repo = new TypeRepo(assemblies);
            meepo = new Meepo.Meepo(listenerAddress, serverAddresses);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="listenerAddress">Address you want to expose</param>
        /// <param name="serverAddresses">List of server addresses to connect to</param>
        /// <param name="logger">Custom ILogger implementation</param>
        /// <param name="assemblies">Assemblies where meepo packets are defined</param>
        public TypicalMeepo(TcpAddress listenerAddress, IEnumerable<TcpAddress> serverAddresses, ILogger logger, IEnumerable<Assembly> assemblies)
        {
            repo = new TypeRepo(assemblies);
            meepo = new Meepo.Meepo(listenerAddress, serverAddresses, logger);
        }
        #endregion

        /// <summary>
        /// Run meepo server.
        /// Starts listening for new clients
        /// and connects to specified servers.
        /// </summary>
        public async Task Start()
        {
            await meepo.Start();
        }

        /// <summary>
        /// Stop meepo server.
        /// </summary>
        public void Stop()
        {
            meepo.Stop();
        }

        /// <summary>
        /// Remove client.
        /// </summary>
        /// <param name="id">Client ID</param>
        public void RemoveClient(Guid id)
        {
            meepo.RemoveClient(id);
        }

        /// <summary>
        /// Send data to a specific client.
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <param name="data">Data to send</param>
        /// <returns></returns>
        public async Task Send<T>(Guid id, T data)
        {
            await meepo.Send(id, data.PackageToBytes(repo));
        }

        /// <summary>
        /// Send data to a all clients.
        /// Including connected servers.
        /// </summary>
        /// <param name="data">Data to send</param>
        /// <returns></returns>
        public async Task Send<T>(T data)
        {
            await meepo.Send(data.PackageToBytes(repo));
        }

        /// <summary>
        /// Get IDs and addresses of all connected servers.
        /// </summary>
        /// <returns></returns>
        public Dictionary<Guid, TcpAddress> GetServerClientInfos()
        {
            return (Dictionary<Guid, TcpAddress>) meepo.GetServerClientInfos();
        }

        /// <summary>
        /// Subscribe to a event when message of a specific type is received.
        /// </summary>
        /// <typeparam name="T">Meepo paskage</typeparam>
        /// <param name="action">MessageReceivedHandler delegate</param>
        public void Subscribe<T>(MessageReceivedHandler<T> action)
        {
            var attribute = typeof(T).GetMeepoPackageAttribute();

            if (attribute == null) throw new TypicalMeepoException("Type must have meepo package attribute!");

            var typeId = repo.GetId(typeof(T));

            handlers[typeId] = x =>
            {
                (int returnTypeId, object obj) = x.Bytes.BytesToPackage(repo);

                if (returnTypeId != typeId) return;

                action(x.Id, (T) obj);
            };

            meepo.MessageReceived += handlers[typeId];
        }

        public void Unsubscribe<T>()
        {
            var typeId = repo.GetId(typeof(T));

            if (!handlers.ContainsKey(typeId)) return;

            meepo.MessageReceived -= handlers[typeId];
        }

        public void Dispose()
        {
            meepo.Stop();
        }
    }
}
