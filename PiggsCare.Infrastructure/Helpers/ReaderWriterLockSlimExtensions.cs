namespace PiggsCare.Infrastructure.Helpers
{
    /// <summary>
    ///     Extension methods for <see cref="ReaderWriterLockSlim" /> to provide disposable read and write locks.
    /// </summary>
    public static class ReaderWriterLockSlimExtensions
    {
        /// <summary>
        ///     Acquires a read lock on the <see cref="ReaderWriterLockSlim" /> and returns an <see cref="IDisposable" />
        ///     that will release the lock when disposed.
        /// </summary>
        /// <param name="rwLock" >The <see cref="ReaderWriterLockSlim" /> instance.</param>
        /// <returns>An <see cref="IDisposable" /> that releases the read lock on dispose.</returns>
        public static IDisposable Read( this ReaderWriterLockSlim rwLock )
        {
            rwLock.EnterReadLock();
            return new Releaser(rwLock, LockType.Read);
        }

        /// <summary>
        ///     Acquires a write lock on the <see cref="ReaderWriterLockSlim" /> and returns an <see cref="IDisposable" />
        ///     that will release the lock when disposed.
        /// </summary>
        /// <param name="rwLock" >The <see cref="ReaderWriterLockSlim" /> instance.</param>
        /// <returns>An <see cref="IDisposable" /> that releases the write lock on dispose.</returns>
        public static IDisposable Write( this ReaderWriterLockSlim rwLock )
        {
            rwLock.EnterWriteLock();
            return new Releaser(rwLock, LockType.Write);
        }

        /// <summary>
        ///     Represents the type of lock held.
        /// </summary>
        private enum LockType
        {
            Read,
            Write
        }

        /// <summary>
        ///     Releases the acquired lock (read or write) when disposed.
        /// </summary>
        private sealed class Releaser:IDisposable
        {
            private readonly LockType _lockType;
            private readonly ReaderWriterLockSlim _rwLock;
            private bool _disposed;

            /// <summary>
            ///     Initializes a new instance of the <see cref="Releaser" /> class.
            /// </summary>
            /// <param name="rwLock" >The <see cref="ReaderWriterLockSlim" /> instance.</param>
            /// <param name="lockType" >The type of lock held.</param>
            public Releaser( ReaderWriterLockSlim rwLock, LockType lockType )
            {
                _rwLock = rwLock;
                _lockType = lockType;
            }

            /// <summary>
            ///     Releases the lock held by this instance.
            /// </summary>
            public void Dispose()
            {
                if (_disposed) return;
                _disposed = true;

                if (_lockType == LockType.Read)
                    _rwLock.ExitReadLock();
                else
                    _rwLock.ExitWriteLock();
            }
        }
    }
}
