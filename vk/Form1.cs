using System;
using System.Text;
using System.Windows.Forms;
using VkNet;
using System.Net;
using System.IO;
using System.Threading;

namespace vk
{
    public partial class VK : Form
    {
        public VK()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private DirectoryInfo dirInfo;
        private VkApi vk;

        private void button1_Click(object sender, EventArgs e)
        {
            vk = new VkApi();
            try
            {
                vk.Authorize(new ApiAuthParams
                {
                    ApplicationId = 5591430,
                    Login = log.Text,
                    Password = pass.Text,
                    Settings = VkNet.Enums.Filters.Settings.Documents
                });
                group1.Visible = false;
                group2.Visible = true;
            }
            catch (Exception)
            {
                label3.Visible = true;
                label3.Text = "Ошибка входа";
            }
        }
                
        private void button1_Click_1(object sender, EventArgs e)
        {
            folder.ShowDialog();
            label5.Text = folder.SelectedPath;
            dirInfo = new DirectoryInfo(folder.SelectedPath);
            label4.Text="Количество файлов:"+dirInfo.GetFiles().Length.ToString();
            var files = dirInfo.GetFiles();
            label5.Visible = true;
            label4.Visible = true;
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            if (dirInfo != null)
            {
                Thread thread = new Thread(Uploading);
                thread.Start();
            }
            else
            {
                return;
            }             
        }

        private void Uploading()
        {
            WebClient client = new WebClient();
            label6.Visible = true;
            var files = dirInfo.GetFiles();

            for (int i = 0; i < files.Length; i++)
            {
                string url = vk.Docs.GetUploadServer().UploadUrl;

                byte[] resArr = client.UploadFile(new Uri(url), dirInfo.FullName + "\\" + files[i].Name);                

                try
                {
                    vk.Docs.Save(Encoding.ASCII.GetString(resArr), files[i].Name, null);
                }

                catch (VkNet.Exception.CaptchaNeededException cEx)
                {
                    client.DownloadFile(cEx.Img, "123.jpg");
                    Captcha cp = new Captcha();
                    cp.ShowDialog();
                    if (cp.DialogResult == DialogResult.OK)
                    {
                        try
                        {
                            vk.Docs.Save(Encoding.ASCII.GetString(resArr), files[i].Name, null, cEx.Sid, cp.captchaSolve);
                        }
                        catch (Exception)
                        {
                            cp.ShowDialog();
                        }
                    }
                    else
                    {
                        cp.ShowDialog();
                    }
                }
                label6.Text = (i+1).ToString() + '/' + files.Length.ToString();             
            }
        }
    }
}
