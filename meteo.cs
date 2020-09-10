using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUN
{
    public partial class meteo : Form
    {
        private prognoza m_pogoda;
        List<string> m_prognoza;
        public meteo(prognoza _pogoda)
        {
                m_pogoda = _pogoda;
                InitializeComponent();
                m_prognoza = m_pogoda.WeatherNow(DateTime.Now.Hour);
                dgvPrognoza.DataSource = m_pogoda;
            //label2.Text = m_pogoda.opady[0];
        }

    }
}
