using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Management;

namespace Task_Manager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ShowProcess();
            ShowThreads();
            label2.Text = allproccesCount().ToString();
        }

        void ShowProcess()
        {
            var allProcess = from pro in Process.GetProcesses(".") orderby pro.Id select pro;
            foreach (var proc in allProcess)
            {
                string[] arr = { "" + proc.Id, "" + proc.ProcessName, "" + proc.WorkingSet64 / 1000000 + "MB", GetNameUser(proc.Id), "" + proc.BasePriority };
                dataGridView1.Rows.Add(arr);
            }
        }

        void CloseProcess()
        {
            int proid = Convert.ToInt32(dataGridView1.CurrentCell.Value.ToString());
            Process myProc = null;
            try
            {
                myProc = Process.GetProcessById(proid);
                myProc.Kill();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public string GetNameUser(int ID)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + ID;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject obj in processList)
            {
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    return argList[0];
                }
            }
            return "System";
        }

        int allproccesCount() 
        {
            var allProcess = from pro in Process.GetProcesses(".") orderby pro.Id select pro;
            return allProcess.Count();
        }

        void ShowThreads()
        {
            Process proc = Process.GetProcessesByName("devenv")[0];
            ProcessThreadCollection processThreads = proc.Threads;

            foreach (ProcessThread thread in processThreads)
            {
                string[] arr = { "" + thread.Id, "" + thread.PriorityLevel, "" + thread.CurrentPriority };
                dataGridView2.Rows.Add(arr);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CloseProcess();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowProcess();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
