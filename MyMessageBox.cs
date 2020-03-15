using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUN
{
    public static class MyMessageBox
    {
        public static System.Windows.Forms.DialogResult ShowMessage(string message, string caption, System.Windows.Forms.MessageBoxButtons button, System.Windows.Forms.MessageBoxIcon icon)
        {
            System.Windows.Forms.DialogResult dlgResult = System.Windows.Forms.DialogResult.None;
            switch (button)
            {
                case System.Windows.Forms.MessageBoxButtons.OK:
                    using (frmMessageOK msgOK = new frmMessageOK())
                    {
                        msgOK.Text = caption;
                        msgOK.Message = message;
                        switch (icon)
                        {
                            case System.Windows.Forms.MessageBoxIcon.Information:
                                msgOK.MessageIcon = RUN.Properties.Resources.OK;
                                break;
                            case System.Windows.Forms.MessageBoxIcon.Question:
                                msgOK.MessageIcon = RUN.Properties.Resources.Question;
                                break;
                            case System.Windows.Forms.MessageBoxIcon.Warning:
                                msgOK.MessageIcon = RUN.Properties.Resources.OK;
                                break;
                        }
                        dlgResult = msgOK.ShowDialog();
                    }
                    break;
                case System.Windows.Forms.MessageBoxButtons.YesNo:
                    using (frmMessageYesNo msgYesNo = new frmMessageYesNo())
                    {
                        msgYesNo.Text = caption;
                        msgYesNo.Message = message;
                        switch (icon)
                        {
                            case System.Windows.Forms.MessageBoxIcon.Information:
                                msgYesNo.MessageIcon = RUN.Properties.Resources.OK;
                                break;
                            case System.Windows.Forms.MessageBoxIcon.Question:
                                msgYesNo.MessageIcon = RUN.Properties.Resources.Question;
                                break;
                        }
                        dlgResult = msgYesNo.ShowDialog();
                    }
                    break;
            }
            return dlgResult;
        }
    }
}
