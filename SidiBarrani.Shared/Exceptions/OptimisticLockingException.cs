using System;

namespace SidiBarrani.Shared.Exceptions
{
    public class OptimisticLockingException : Exception
    {
        public Guid ClientVersionId { get; }
        public Guid ServerVersionId { get; }

        public OptimisticLockingException(Guid clientVersionId, Guid serverVersionId)
        : base($"Version missmatch ({nameof(ClientVersionId)} = {clientVersionId} != {serverVersionId} = {nameof(ServerVersionId)})!")
        {
            ClientVersionId = clientVersionId;
            ServerVersionId = serverVersionId;
        }
    }
}
