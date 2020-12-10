using System;
using Cysharp.Threading.Tasks;

namespace Net.HungryBug.Core.Loading
{
    public interface ILoadingContent
    {
        /// <summary>
        /// Inform the changed of <see cref="ILoadingContent.Progress"/>.
        /// </summary>
        event Action<byte> OnProgressChanged;

        /// <summary>
        /// Gets the loading progress. (Raise property changed)
        /// </summary>
        byte Progress { get; }

        /// <summary>
        /// Run all contents.
        /// </summary>
        UniTask Run();
    }
}
