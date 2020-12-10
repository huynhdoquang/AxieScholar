using System;
using Cysharp.Threading.Tasks;

namespace Net.HungryBug.Core.Loading
{
    public class SequenceContent : LoadingContent
    {

        /// <summary>
        /// Run all <see cref="LoadingContent.contents"/> sequentially.
        /// </summary>
        protected override async UniTask RunContents()
        {
            foreach (var content in this.contents.Keys)
            {
                await UniTask.Yield();
                byte lastProgress = 0;
                Action<byte> n = (progress) =>
                {
                    this.UpdateProgress(content, (byte)(progress - lastProgress));
                    lastProgress = progress;
                };

                content.OnProgressChanged += n;
                await content.Run();
                content.OnProgressChanged -= n;
            }
        }
    }
}
