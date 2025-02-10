namespace MyPage.Models
{
    public class LayoutSetting
    {
        private bool Is100Vh;
        private bool IsNoNav;

        public event Action LayoutSettingChanged;

        public void GetData(out bool _is100vh, out bool _isnovav)
        {
            _is100vh = Is100Vh;
            _isnovav = IsNoNav;
        }

        public void SetData(bool _is100vh, bool _isnovav)
        {
            Is100Vh = _is100vh;
            IsNoNav = _isnovav;
            LayoutSettingChanged?.Invoke();
        }
    }
}
