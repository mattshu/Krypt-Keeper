using System.Collections.Generic;

namespace KryptKeeper
{
    internal class ProgressPacket
    {
        private long _currentBytesState { get; }
        private long _currentBytesTotal { get; }
        private long _totalBytesState { get; }
        private long _totalBytesTotal { get; }
        private int _totalFilesState { get; }
        private int _totalFilesTotal { get; }

        public ProgressPacket(IReadOnlyList<long> currentBytes, IReadOnlyList<int> totalFiles, IReadOnlyList<long> totalBytes)
        {
            // [0:progressState, 1:progressTotal]
            _currentBytesState = currentBytes[0];
            _currentBytesTotal = currentBytes[1];
            _totalFilesState = totalFiles[0];
            _totalFilesTotal = totalFiles[1];
            _totalBytesState = totalBytes[0];
            _totalBytesTotal = totalBytes[1];
        }

        public int GetCurrentFileProgress() =>
            Utils.GetPercentProgress(_currentBytesState, _currentBytesTotal);

        public int GetTotalFilesProgress() =>
            Utils.GetPercentProgress(_totalFilesState, _totalFilesTotal);

        public int GetTotalBytesProgress() =>
            Utils.GetPercentProgress(_totalBytesState, _totalBytesTotal);
    }
}
