using System.IO;
using System.Windows.Forms;


namespace dRz.Test.OpenDwg
{
    public class Services
    {


        /// <summary>
        /// путь к папке
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>путь к папке</returns>
        internal static string Browser(string description = "Open folder")
        {
            //https://autolisp.ru/2024/05/23/nanocad-net-select-folder/
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = description;
#if !AC24
                dlg.UseDescriptionForTitle = true;
#endif
                //dlg.Multiselect  = true;
                // Остальные настройки

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (!string.IsNullOrWhiteSpace(dlg.SelectedPath))
                    {
                        return dlg.SelectedPath;
                    }
                }
                else
                {
                    MessageBox.Show("Не выбран каталог!",
                                    "Test",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                }

                return string.Empty;
            }

        }


        /// <summary>
        /// Получить пути файлов из каталога
        /// </summary>
        /// <param name="sPath">The s path.</param>
        /// <param name="WithSubfolders">if set to <c>true</c> [with subfolders].</param>
        /// <param name="sSerchPatern">The s serch patern.</param>
        /// <returns>Список файлов</returns>
        internal static string[] GetFilesOfDir(string sPath, bool WithSubfolders = false, string sSerchPatern = "*.dwg")
        {
            try
            {
                if (Directory.Exists(sPath))
                {
                    return Directory.GetFiles(sPath,
                                                sSerchPatern,
                                                (WithSubfolders
                                                ? SearchOption.AllDirectories
                                                : SearchOption.TopDirectoryOnly));
                }
                else
                {
                    return new string[] { };
                }
            }
            catch (System.Exception ex)
            {

                MessageBox.Show(ex.Message,
                                "Test",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                return new string[] { };
            }

        }
    }
}
