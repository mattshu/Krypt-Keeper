using System.Collections.Generic;

namespace KryptKeeper
{
    internal class ProgressPacket
    {
        private long _currentPayloadState { get; }
        private long _currentPayloadTotal { get; }
        private long _totalPayloadState { get; }
        private long _totalPayloadTotal { get; }
        private int _totalFilesState { get; }
        private int _totalFilesTotal { get; }

        public ProgressPacket(IReadOnlyList<long> currentPayload, IReadOnlyList<long> totalPayload, IReadOnlyList<int> totalFiles)
        {
            // [0:progressState, 1:progressTotal]
            _currentPayloadState = currentPayload[0];
            _currentPayloadTotal = currentPayload[1];
            _totalPayloadState = totalPayload[0];
            _totalPayloadTotal = totalPayload[1];
            _totalFilesState = totalFiles[0];
            _totalFilesTotal = totalFiles[1];
        }

        public int GetCurrentFileProgress() =>
            Utils.GetPercentProgress(_currentPayloadState, _currentPayloadTotal);

        public int GetTotalPayloadProgress() =>
            Utils.GetPercentProgress(_totalPayloadState, _totalPayloadTotal);

        public int GetTotalFilesProgress() =>
            Utils.GetPercentProgress(_totalFilesState, _totalFilesTotal);
    }
}
