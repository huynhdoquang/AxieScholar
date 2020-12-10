using Net.HungryBug.Core.Reactive;

namespace Net.HungryBug.Core.Loading.Progress
{
    public interface IProgress
    {
        /// <summary>
        /// Report to chanel.
        /// </summary>
        string Chanel { get; }

        /// <summary>
        /// Gets the name of task.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The progress report, in range (0f -> 1f).
        /// </summary>
        ReactiveProperty<float> Progress { get; }

        /// <summary>
        /// The value indicate if the task has been finished.
        /// </summary>
        ReactiveProperty<bool> IsDone { get; }
    }
}
