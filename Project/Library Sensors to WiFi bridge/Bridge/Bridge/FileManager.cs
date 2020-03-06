using System;
using System.IO;
using System.Windows.Forms;

namespace Bridge
{
    public class FileManager
    {
        private static FileManager INSTANCE = null;

        public string FilePath { get; set; }

        private FileManager()
        {
        }

        public static FileManager getInstance()
        {
            if (INSTANCE == null)
            {
                INSTANCE = new FileManager();
            }
            return INSTANCE;
        }

        public static string readTxtFile(string filePath)
        {
            if (File.Exists(@filePath))
            {
                return File.ReadAllText(@filePath);
            }
            return null;
        }

        public static void writeCollectionToFile(string filePath, System.Windows.Forms.ListBox.ObjectCollection objectCollection)
        {
            using (StreamWriter file = new StreamWriter(@filePath))
            {
                foreach (var item in objectCollection)
                {
                    file.WriteLine(item);
                }
            }
        }

        public static void writeTxtToFile(string filePath, string text)
        {
            using (StreamWriter file = new StreamWriter(@filePath))
            {
                file.Write(text);
            }
        }

        public static void writeLineToFile(string filePath, string text)
        {
            File.AppendAllText(@filePath, text);
        }

        public void selectFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "/home";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FilePath = openFileDialog.FileName;
                }
            }
        }

        public long fileSize()
        {
            using (var stream = File.OpenRead(@FilePath))
            {
                return stream.Length;
            }
        }

    }
}
