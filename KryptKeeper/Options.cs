using KryptKeeper.Properties;

namespace KryptKeeper
{ 
    public class Options
    {
        private readonly MainWindow _main;

        public Options(MainWindow main)
        {
            _main = main;
        }

        public void Load()
        { // TODO https://stackoverflow.com/questions/23924183/keep-users-settings-after-altering-assembly-file-version
            var settings = Settings.Default;
            if (!settings.rememberSettings)
                Reset();
            _main.MaskFileInformation = settings.encryptionMaskFileName || settings.encryptionMaskFileDate;
            _main.MaskFileName = settings.encryptionMaskFileName;
            _main.MaskFileDate = settings.encryptionMaskFileDate;
            _main.ProcessInOrder = settings.processInOrder;
            _main.ProcessInOrderBy = settings.processInOrderBy;
            _main.ProcessInOrderDesc = settings.processInOrderDesc;
            _main.RemoveAfterEncryption = settings.removeAfterEncryption;
            _main.RemoveAfterDecryption = settings.removeAfterDecryption;
            _main.RememberSettings = settings.rememberSettings;
            _main.ConfirmOnExit = settings.confirmOnExit;
        }

        public void Save()
        {
            if (!_main.RememberSettings) return;
            var settings = Settings.Default;
            settings.encryptionMaskFileName = _main.MaskFileInformation && _main.MaskFileName;
            settings.encryptionMaskFileDate = _main.MaskFileInformation && _main.MaskFileDate;
            settings.removeAfterDecryption = _main.RemoveAfterDecryption;
            settings.removeAfterEncryption = _main.RemoveAfterEncryption;
            settings.processInOrder = _main.ProcessInOrder;
            settings.processInOrderBy = _main.ProcessInOrderBy;
            settings.rememberSettings = true; // always true in case user exits with saving
            settings.confirmOnExit = _main.ConfirmOnExit;
            settings.Save();
        }

        public void Reset()
        {
            var settings = Settings.Default;
            settings.encryptionMaskFileName = false;
            settings.encryptionMaskFileDate = false;
            settings.removeAfterEncryption = true;
            settings.removeAfterDecryption = true;
            settings.rememberSettings = false;
            settings.processInOrder = false;
            settings.processInOrderBy = 0;
            settings.processInOrderDesc = false;
            settings.confirmOnExit = true;
            settings.minimizeToTrayOnClose = true;
            settings.Save();
        }
        // test
    }
}
