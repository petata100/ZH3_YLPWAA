using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace FormsApp
{
    public partial class ucExcel : UserControl
    {
        Excel.Application xlApp;
        Excel.Workbook xlWb;
        Excel.Worksheet xlSheet;

        public ucExcel()
        {
            InitializeComponent();
        }

        public void CreateTable()
        {
            string[] fejlécek = new string[]
            {
                "MennyisegiEgysegId",
                "EgysegNev"
            };

            for (int i = 0; i < fejlécek.Count(); i++)
            {
                xlSheet.Cells[1, i + 1] = fejlécek[i];
            }

            Models.ReceptContext context = new();
            var megysegek = context.MennyisegiEgysegek.ToList();

            int sorokSzama = megysegek.Count();
            int oszlopokSzama = fejlécek.Count();

            object[,] adatTomb = new object[sorokSzama, oszlopokSzama];

            for (int i = 0; i < megysegek.Count(); i++)
            {
                adatTomb[i, 0] = megysegek[i].MennyisegiEgysegId;
                adatTomb[i, 1] = megysegek[i].EgysegNev;
            }

            Excel.Range adatRange = xlSheet.get_Range("A2", Type.Missing).get_Resize(sorokSzama, oszlopokSzama);
            adatRange.Value2 = adatTomb;

            Excel.Range fejlecRange = xlSheet.get_Range("A1", Type.Missing).get_Resize(1, oszlopokSzama);
            fejlecRange.Font.Bold = true;
        }

        public void CreateExcel()
        {
            try
            {
                xlApp = new Excel.Application();
                xlWb = xlApp.Workbooks.Add(Missing.Value);
                xlSheet = xlWb.ActiveSheet;

                CreateTable();

                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                xlWb.Close(false, Type.Missing, Type.Missing);
                xlApp.Quit();
                xlApp = null;
                xlWb = null;
            }
        }

        private void bExport_Click(object sender, EventArgs e)
        {
            CreateExcel();
        }
    }
}


