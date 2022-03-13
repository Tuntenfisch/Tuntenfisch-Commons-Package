using System;
using System.Runtime.InteropServices;
using System.Threading;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Tuntenfisch.Commons.Collections.Native
{
    // We cannot use C#'s regular lock mechanism inside jobs because jobs don't allow managed objects,
    // So we use a spin lock as described in https://stackoverflow.com/questions/23842935/c-sharp-interlocked-functions-as-a-lock-mechanism.
    // If this is actually performant is another question entirely...
    [StructLayout(LayoutKind.Sequential)]
    [NativeContainer]
    [NativeContainerIsAtomicWriteOnly]
    public unsafe struct NativeLock : IDisposable
    {
        #region Public Properties
        public bool IsCreated => m_locked != null;
        #endregion

        #region Private Fields
        private readonly Allocator m_allocator;

        [NativeDisableUnsafePtrRestriction]
        private int* m_locked;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        AtomicSafetyHandle m_Safety;
        [NativeSetClassTypeToNullOnSchedule]
        DisposeSentinel m_DisposeSentinel;
#endif
        #endregion

        #region Public Methods
        public NativeLock(Allocator allocator)
        {
            m_allocator = allocator;
            m_locked = (int*)UnsafeUtility.Malloc(UnsafeUtility.SizeOf<int>(), UnsafeUtility.AlignOf<int>(), allocator);
            *m_locked = 0;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            DisposeSentinel.Create(out m_Safety, out m_DisposeSentinel, 0, allocator);
#endif
        }

        public void Acquire()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
#endif
            while (Interlocked.CompareExchange(ref *m_locked, 1, 0) != 0) continue;
        }

        public void Release()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
#endif
            *m_locked = 0;
        }

        public void Dispose()
        {
            UnsafeUtility.Free(m_locked, m_allocator);
            m_locked = null;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            DisposeSentinel.Dispose(ref m_Safety, ref m_DisposeSentinel);
#endif
        }
        #endregion
    }
}