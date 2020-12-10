using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Net.HungryBug.Core.Loading
{
    public class ParallelContent : LoadingContent
    {
        /// <summary>
        /// Run all <see cref="LoadingContent.contents"/> parallelly.
        /// </summary>
        protected override async UniTask RunContents()
        {
            var tasks = new List<UniTask>(this.contents.Count);
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

                var task = content.Run();
                var t = task.ContinueWith(() => content.OnProgressChanged -= n);
                tasks.Add(task);
            }

            //wait for all.
            await UniTask.WhenAll(tasks);
        }
    }
}
