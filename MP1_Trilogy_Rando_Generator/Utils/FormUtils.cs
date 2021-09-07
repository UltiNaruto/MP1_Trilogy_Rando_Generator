using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace MP1_Trilogy_Rando_Generator
{
    class FormUtils
    {
        #region WinAPI Constants
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;
        #endregion

        #region WinAPI Imports
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        #endregion

        static main_form Instance = null;

        internal static void Init(main_form form)
        {
            Instance = form;
        }

        internal static void SendClickToCaption()
        {
            if (Instance == null) {
                throw new Exception("Call FormUtils.Init() in your form constructor");
            }
            ReleaseCapture();
            SendMessage(Instance.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        internal static void SafeClose()
        {
            if (Instance == null) {
                throw new Exception("Call FormUtils.Init() in your form constructor");
            }

            if (Instance.InvokeRequired) {
                Instance.Invoke(new Action(() => SafeClose()));
            } else {
                Instance.Close();
            }
        }

        internal static Object GetComboBoxSelectedItem(ComboBox comboBox)
        {
            if (Instance.InvokeRequired)
                return Instance.Invoke(new Func<Object>(() => GetComboBoxSelectedItem(comboBox)));
            else
                return comboBox.SelectedItem;
        }

        internal static void SetProgressStatus(int cur, int max)
        {
            if (Instance.InvokeRequired)
            {
                Instance.Invoke(new Action(() => SetProgressStatus(cur, max)));
            }
            else
            {
                if (cur != max)
                {
                    Instance.status_progress_bar.Visible = true;
                    Instance.status_progress_bar.Value = (cur * 100) / (max - 1);
                }
                else
                {
                    Instance.status_progress_bar.Visible = false;
                    Instance.status_progress_bar.Value = 0;
                }
                Instance.status_progress_bar.Update();
            }
        }

        internal static void SetStatus(String status, int cur = 0, int len = 0)
        {
            if (Instance.InvokeRequired)
            {
                Instance.Invoke(new Action(() => SetStatus(status, cur, len)));
            }
            else
            {
                Instance.status_lbl.Text = "Status : " + status;
                if (len > 1)
                    Instance.status_lbl.Text += " (" + (cur + 1) + " / " + len + ")";
                Instance.status_lbl.Update();
            }
        }

        internal static String GetControlText(Control control)
        {
            if (Instance.InvokeRequired)
                return (String)Instance.Invoke(new Func<String>(() => GetControlText(control)));
            else
                return control.Text;
        }

        internal static void SetControlText(Control control, String text)
        {
            if (Instance == null)
            {
                throw new Exception("Call FormUtils.Init() in your form constructor");
            }

            if (Instance.InvokeRequired)
            {
                Instance.Invoke(new Action(() => SetControlText(control, text)));
            }
            else
            {
                control.Text = text;
            }
        }

        internal static void SetControlEnabled(Control control, bool enabled)
        {
            if (Instance == null)
            {
                throw new Exception("Call FormUtils.Init() in your form constructor");
            }

            if (Instance.InvokeRequired)
            {
                Instance.Invoke(new Action(() => SetControlEnabled(control, enabled)));
            }
            else
            {
                control.Enabled = enabled;
            }
        }

        internal static void SwitchTab(int selected)
        {
            if (Instance == null) {
                throw new Exception("Call FormUtils.Init() in your form constructor");
            }

            if (Instance.InvokeRequired) {
                Instance.Invoke(new Action(() => SwitchTab(selected)));
            } else {
                if (!Instance.isLoading)
                {
                    // Template tab selected
                    Instance.template_iso_btn.Enabled = selected != 0;
                    Instance.template_iso_btn.BackColor = selected == 0 ? Color.Gray : Color.FromArgb(60, 63, 65);
                    // Randomizer tab selected
                    Instance.randomizer_btn.Enabled = selected != 1;
                    Instance.randomizer_btn.BackColor = selected == 1 ? Color.Gray : Color.FromArgb(60, 63, 65);
                    // Settings tab selected
                    Instance.settings_btn.Enabled = selected != 2;
                    Instance.settings_btn.BackColor = selected == 2 ? Color.Gray : Color.FromArgb(60, 63, 65);
                    // Extras tab selected
                    Instance.extras_btn.Enabled = selected != 3;
                    Instance.extras_btn.BackColor = selected == 3 ? Color.Gray : Color.FromArgb(60, 63, 65);

                    Instance.tab_manager.SelectedIndex = selected;
                    Instance.tab_manager.Update();
                }
            }
        }

        internal static void SwitchSettingsTab(int selected)
        {
            if (Instance == null) {
                throw new Exception("Call FormUtils.Init() in your form constructor");
            }

            if (Instance.InvokeRequired) {
                Instance.Invoke(new Action(() => SwitchSettingsTab(selected)));
            } else {
                if (!Instance.isLoading) {
                    // Controls tab selected
                    Instance.controller_btn.Enabled = selected != 0;
                    Instance.controller_btn.BackColor = selected == 0 ? Color.Gray : Color.FromArgb(60, 63, 65);
                    // Display tab selected
                    Instance.display_btn.Enabled = selected != 1;
                    Instance.display_btn.BackColor = selected == 1 ? Color.Gray : Color.FromArgb(60, 63, 65);
                    // Visor tab selected
                    Instance.visor_btn.Enabled = selected != 2;
                    Instance.visor_btn.BackColor = selected == 2 ? Color.Gray : Color.FromArgb(60, 63, 65);
                    // Sound tab selected
                    Instance.sound_btn.Enabled = selected != 3;
                    Instance.sound_btn.BackColor = selected == 3 ? Color.Gray : Color.FromArgb(60, 63, 65);

                    Instance.settings_tab_manager.SelectedIndex = selected;
                    Instance.settings_tab_manager.Update();
                }
            }
        }

        internal static Thread RunAsynchrousTask(Action action)
        {
            Thread thread = new Thread(new ThreadStart(() => action.Invoke()));
            thread.Start();
            return thread;
        }

        internal static Thread RunRepeatingAsynchrousTask(Action action)
        {
            Thread thread = new Thread(new ParameterizedThreadStart((t) => {
                while (((Thread)t).ThreadState != ThreadState.AbortRequested) {
                    action.Invoke();
                }
            }));
            thread.Start(thread);
            return thread;
        }

        internal static void ShowMessageBox(String message)
        {
            if (Instance == null)
            {
                throw new Exception("Call FormUtils.Init() in your form constructor");
            }

            if (Instance.InvokeRequired)
                Instance.Invoke(new Action(() => MessageBox.Show(message)));
        }
    }
}
