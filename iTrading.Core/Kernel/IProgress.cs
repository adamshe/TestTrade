namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Progress indicator. 
    /// See <seealso cref="T:iTrading.Core.Kernel.ProgressForm" /> for default implementation.
    /// </summary>
    public interface IProgress
    {
        /// <summary>
        /// Called on abort the current processing.
        /// Please note: Event handlers have to be attached after every call of <see cref="M:iTrading.Core.Kernel.IProgress.Terminate" />.
        /// </summary>
        event AbortEventHandler Aborted;

        /// <summary>
        /// Initialize the progress indication.
        /// </summary>
        /// <param name="maxSteps">Number of steps to indicate. Set to 0, if the steps are not countable.</param>
        /// <param name="abortable">Is progress indication abortable ?</param>
        void Initialise(int maxSteps, bool abortable);
        /// <summary>
        /// Perform a progress step.
        /// </summary>
        void PerformStep();
        /// <summary>
        /// Terminate the progress indication.
        /// </summary>
        void Terminate();

        /// <summary>
        /// Was progress indication aborted ?
        /// </summary>
        bool IsAborted { get; }

        /// <summary>
        /// Get/set display text.
        /// </summary>
        string Message { get; set; }
    }
}

