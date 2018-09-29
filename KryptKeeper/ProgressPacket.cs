using System.Collections.Generic;

namespace KryptKeeper
{
    internal class ProgressPacket
    {
        private readonly long _currentBytesState;
        private readonly long _currentBytesTotal;
        private readonly long _totalBytesState;
        private readonly long _totalBytesTotal;
        private readonly int _totalFilesState;
        private readonly int _totalFilesTotal;

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
