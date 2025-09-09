using System;
using Cysharp.Threading.Tasks;
using Quality.Core.Logger;

namespace Quality.Core.Utilities
{
    public enum ProcessingState
    {
        IDLE,
        PROCESSING
    }

    public class ProcessingTracker
    {
        private int                     _taskCount;
        private ProcessingState         _state;
        private Action<ProcessingState> _onProcessStateChanged;

        public bool IsBusy => _state == ProcessingState.PROCESSING;

        public ProcessingTracker() { }

        public ProcessingTracker(Action<ProcessingState> onProcessStateChanged)
        {
            _onProcessStateChanged = onProcessStateChanged;
        }

        public async UniTask AutoTracker(UniTask task)
        {
            TaskStarting();

            try
            {
                await task;
            }
            finally
            {
                TaskCompleted();
            }
        }

        public async UniTask<T> AutoTracker<T>(UniTask<T> task)
        {
            TaskStarting();

            try
            {
                return await task;
            }
            finally
            {
                TaskCompleted();
            }
        }

        public void AutoTrackerForget(UniTask task)
        {
            AutoTracker(task).Forget();
        }

        public void Reset()
        {
            if (IsBusy)
            {
                this.LogWarning("ProcessingTracker was reset while still busy.");
            }

            _taskCount = 0;

            if (_state != ProcessingState.IDLE)
            {
                _state = ProcessingState.IDLE;
                _onProcessStateChanged?.Invoke(_state);
            }
        }

        private void TaskStarting()
        {
            _taskCount++;

            if (_taskCount == 1)
            {
                _state = ProcessingState.PROCESSING;
                _onProcessStateChanged?.Invoke(_state);
            }
        }

        private void TaskCompleted()
        {
            _taskCount--;

            if (_taskCount == 0)
            {
                _state = ProcessingState.IDLE;
                _onProcessStateChanged?.Invoke(_state);
            }
        }
    }
}
