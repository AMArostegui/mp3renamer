using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Tags.ID3;
using Test_AM.Properties;

namespace Test_AM
{
    /// <summary>
    /// Ventana principal de la aplicación.
    /// </summary>
    public partial class Mp3RenamerForm : Form
    {
        /// <summary>
        /// Archivos seleccionados.
        /// </summary>
        private readonly BindingList<Mp3InfoGui> selectedFiles;

        /// <summary>
        /// Inicializa una instancia de la clase <see cref="Mp3RenamerForm"/>.
        /// </summary>
        public Mp3RenamerForm()
        {
            InitializeComponent();

            this.selectedFiles = new BindingList<Mp3InfoGui>();
            this.grid.DataBindingComplete += this.OnGridDataBindingComplete;
            this.grid.DragEnter += this.OnDragEnter;
            this.grid.DragDrop += this.OnDragDrop;
            this.addFilesLink.DragEnter += this.OnDragEnter;
            this.addFilesLink.DragDrop += this.OnDragDrop;
        }

        #region Rename methods

        /// <summary>
        /// Modifica <paramref name="text"/> acorde a lo especificado en <paramref name="type"/>.
        /// </summary>
        /// <param name="text">Texto a modificar.</param>
        /// <param name="type">Tipo de modificación a aplicar.</param>
        /// <param name="intelligentMode">Aplica algunas excepciones a determinados modos.</param>
        /// <param name="removeTrackNumber">Indica si eliminar el núemro de canción.</param>
        /// <returns>El texto modificado según <paramref name="type"/>.</returns>
        private static string Rename(
            string text, RenameType type, bool intelligentMode, bool removeTrackNumber)
        {
            // Buscar la posición donde acaba el número de pista
            int indexTrackNo = 0;
            while (indexTrackNo < text.Length && text[indexTrackNo] >= '0' && text[indexTrackNo] <= '9')
            {
                indexTrackNo++;
            }

            // Entre el número de pista y el título de la canción solo debe haber un espacio
            if (indexTrackNo > 0 && indexTrackNo < text.Length)
            {
                int index2 = indexTrackNo;
                StringBuilder builder = new StringBuilder(text);
                builder[index2] = ' ';
                index2++;
                while (index2 < builder.Length)
                {
                    switch (builder[index2])
                    {
                        // Algunas otras cosas que usa la peña para separar el número de pista del título
                        case ' ':
                        case '-':
                        case '.':
                            builder = builder.Remove(index2, 1);
                            break;

                        // Ya es el título de la pista; salir del bucle
                        default:
                            index2 = builder.Length;
                            break;
                    }                    
                }
                text = builder.ToString();
            }

            if (removeTrackNumber)
            {
                if (indexTrackNo > 0)
                {
                    text = text.Remove(0, indexTrackNo).TrimStart();
                }
                else
                {
                    indexTrackNo = text.Length - 1;
                    while (indexTrackNo >= 0 && text[indexTrackNo] >= '1' && text[indexTrackNo] <= '0')
                    {
                        indexTrackNo--;
                    }

                    if (indexTrackNo < text.Length - 1)
                    {
                        text = text.Remove(indexTrackNo + 1).TrimEnd();
                    }
                }
            }

            switch (type)
            {
                case RenameType.Upper:
                    text = text.ToUpper();
                    break;

                case RenameType.Lower:
                    text = text.ToLower();
                    break;

                case RenameType.WordUpper:
                    string value = text.ToLower();
                    if (value.Length > 0)
                    {
                        StringBuilder builder = new StringBuilder(value);
                        builder[0] = value[0].ToString().ToUpper()[0];

                        int index = NextWord(value, 0);
                        while (index >= 0 && index + 1 < value.Length)
                        {
                            index++;
                            if (value[index] == '(' || value[index] == '[')
                                index++;
                            builder[index] = value[index].ToString().ToUpper()[0];
                            index = NextWord(value, index);
                        }

                        value = builder.ToString();
                    }

                    if (intelligentMode)
                    {
                        ApplyRenameExceptions(ref value);
                    }

                    text = value;
                    break;

                case RenameType.SentenceUpper:
                    if (text.Length > 1)
                    {
                        value = text.ToLower();
                        value = value[0].ToString().ToUpper() + value.Substring(1);
                        if (intelligentMode)
                        {
                            ApplyRenameExceptions(ref value);
                        }

                        return value;
                    }

                    text = text.ToUpper();
                    break;
            }

            return text;
        }

        /// <summary>
        /// Busca la posición de la siguiente palabra en la cadena. 
        /// </summary>
        /// <param name="Text">Cadena que está siendo parseada</param>
        private static int NextWord(string value, int start)
        {
            // Las palabras pueden estar separadas por espacios o por puntos
            int indexspc = value.IndexOf(' ', start);
            int indexdot = value.IndexOf('.', start);
            if (indexdot == -1)
                return indexspc;
            else if (indexspc == -1)
                return indexdot;
            else
                return Math.Min(indexspc, indexdot);
        }

        /// <summary>
        /// Aplica algunas excepciones. 
        /// </summary>
        /// <param name="value">Texto a actualizar.</param>
        private static void ApplyRenameExceptions(ref string value)
        {
            if (value.Length == 0)
                return;

            List<string> alwaysLower = new List<string>
            {
                "a", "adentro", "al", "con", "de", "del", "el", "ella", "ellas", "ellos", "en", 
                "fue", "la", "las", "lo", "los", "nosotros", "nosotras", "por", "que", "sin", "tu", 
                "tras", "un", "una", "unas", "uno", "unos", "vosotros", "vosotras", "y", "yo",
                "an", "and", "another", "am", "at", "are", "be", "but", "by", "do", "don't", "en", 
                "end", "for", "go", "going", "he", "had", "hadn't", "has", "hasn's", "have", "haven't", 
                "if", "in", "is", "it", "like", "me", "of", "off", "on", "out", "she", "the", "they", 
                "this", "to", "up", "was", "wasn't", "were", "weren't", "will", "with", "without", 
                "would", "wouldn't", "you",
            };

            StringBuilder builder = new StringBuilder();
            string[] words = value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Buscar la primera palabra que no sea un número
            int idxWord = 0;
            int aux;            
            while (idxWord < words.Length && int.TryParse(words[idxWord], out aux))
                idxWord++;
            idxWord++;

            // Copiar en la cadena resultante todos los numericos
            builder.Append(words[0]);
            for (int i = 1; i < words.Length && i < idxWord; i++)
                builder.Append(" " + words[i]);

            // Aplicar las excepciones a partir de la primera palabra
            for (; idxWord < words.Length; idxWord++)
            {
                if (string.Compare(words[idxWord], "I", true) == 0)
                {
                    builder.Append(" I");
                }
                else if (string.Compare(words[idxWord], "I'm", true) == 0)
                {
                    builder.Append(" I'm");
                }
                else if (alwaysLower.Contains(words[idxWord].ToLower()))
                {
                    // Las excepciones se ponen en minúscula solo si no llevan un guión delante
                    if (idxWord > 0 && words[idxWord - 1] == "-")
                        builder.AppendFormat(" {0}", words[idxWord]);
                    else
                        builder.AppendFormat(" {0}", words[idxWord].ToLower());
                }
                else
                {
                    builder.AppendFormat(" {0}", words[idxWord]);
                }
            }

            value = builder.ToString();
        }

        /// <summary>
        /// Modifica <paramref name="text"/> acorde a lo especificado en <paramref name="type"/>.
        /// </summary>
        /// <param name="text">Texto a modificar.</param>
        /// <param name="type">Tipo de modificación a aplicar.</param>
        /// <returns>El texto modificado según <paramref name="type"/>.</returns>
        private string Rename(string text, RenameType type)
        {
            return Rename(text, type, this.intelligentConversionMenuItem.Checked, this.removeTrackNumberMenuItem.Checked);    
        }

        /// <summary>
        /// Renombra las celdas seleccionadas del grid.
        /// </summary>
        /// <param name="type">Tipo de modificación a aplicar.</param>
        /// <param name="onlySelected">Indica si aplicar a las celdas seleccionadas o al grid.
        /// </param>
        private void RenameCells(RenameType type, bool onlySelected)
        {
            if (type == RenameType.None)
            {
                return;
            }

            if (onlySelected)
            {
                foreach (DataGridViewCell cell in this.grid.SelectedCells)
                {
                    this.RenameCell(cell, type);
                }
            }
            else
            {
                foreach (DataGridViewRow row in this.grid.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        this.RenameCell(cell, type);
                    }
                }
            }
        }

        /// <summary>
        /// Renombra la celda indicada.
        /// </summary>
        /// <param name="cell">Celda a renombrar.</param>
        /// <param name="type">Tipo de modificación a aplicar.</param>
        private void RenameCell(DataGridViewCell cell, RenameType type)
        {
            DataGridViewColumn column = this.grid.Columns[cell.ColumnIndex];
            if (cell.Value != null)
            {
                if (!column.ReadOnly)
                {
                    cell.Value = Rename(cell.Value.ToString(), type);
                }
                else if (string.Compare(column.Name, "FileName") == 0)
                {
                    Mp3InfoGui info = this.grid.Rows[cell.RowIndex].DataBoundItem as Mp3InfoGui;
                    if (info != null)
                    {
                        info.FileName = Rename(info.FileName, type);
                    }
                }
            }
        }

        /// <summary>
        /// Devuelve el tipo de renombrado activo.
        /// </summary>
        /// <returns>El tipo de renombrado activo.</returns>
        private RenameType GetCurrentRenameType()
        {
            if (this.uppercaseMenuItem.Checked)
            {
                return RenameType.Upper;
            }
            
            if (this.firstLetterUpperMenuItem.Checked)
            {
                return RenameType.WordUpper;
            }
            
            if (this.sentenceCaseMenuItem.Checked)
            {
                return RenameType.SentenceUpper;
            }
            
            if (this.lowerCaseMenuItem.Checked)
            {
                return RenameType.Lower;
            }

            return RenameType.None;
        }

        /// <summary>
        /// Crea un objeto <see cref="Mp3InfoGui"/> a partir de un archivo.
        /// </summary>
        /// <param name="file">Ruta al archivo.</param>
        /// <param name="pattern">Patrón a utilizar o <c>null</c> para usar el seleccionado.</param>
        /// <returns>Un objeto <see cref="Mp3InfoGui"/>.</returns>
        private Mp3InfoGui CreateMp3Info(string file, ToolStripMenuItem pattern)
        {
            Mp3InfoGui info = new Mp3InfoGui(file);
            if (this.ApplyPattern(info, pattern))
            {
                info.FileName = Rename(info.FileName, RenameType.WordUpper, true, false);
                info.Album = Rename(info.Album, RenameType.WordUpper);
                info.Title = Rename(info.Title, RenameType.WordUpper, this.intelligentConversionMenuItem.Checked, true);
                info.Artist = Rename(info.Artist, RenameType.WordUpper);                
                return info;
            }

            return null;
        }

        /// <summary>
        /// Detecta el patrón a aplicar al objeto <paramref name="info"/>.
        /// </summary>
        /// <param name="info">Objeto <see cref="Mp3InfoGui"/>.</param>
        /// <returns>Un objeto <see cref="ToolStripMenuItem"/> que identifica el patrón a utilizar..</returns>
        private ToolStripMenuItem DetectMenuItemPattern(Mp3InfoGui info)
        {
            string file = info.GetFullPath();
            string filename = Path.GetFileNameWithoutExtension(file);
            if (string.IsNullOrEmpty(filename))
            {
                return null;
            }

            ToolStripMenuItem automaticItem = null;
            if (this.patternAutomaticMenuItem.Checked)
            {
                int index = filename.IndexOf("-");

                if (index > 0)
                {
                    // Hay casos en los que aunque haya un guión, se utiliza como separación del número
                    // de pista; no quiere decir que se este indicando el artista
                    bool foundChar = false;
                    for (int i = index; i > 0; i--)
                    {                        
                        if (Char.IsLetter(filename[i]))
                        {
                            foundChar = true;
                            break;
                        }
                    }

                    // El guión indica el artista de la canción
                    if (foundChar)
                        automaticItem = this.patternDashMenuItem;
                }

                if (automaticItem == null)
                {
                    DirectoryInfo directory = new DirectoryInfo(file);
                    while (directory.Parent != null)
                    {
                        string name = directory.Parent.Name;
                        index = name.IndexOf("-");
                        if (index > 0)
                        {
                            // El nombre del directorio padre tiene un guion...
                            automaticItem = this.patternArtistAlbumFolderMenuItem;
                            break;
                        }

                        directory = directory.Parent;
                    }
                }

                if (automaticItem == null)
                {
                    automaticItem = this.patternFolderToolStripMenuItem;
                }
            }

            return automaticItem;
        }

        /// <summary>
        /// Aplica el patrón seleccionado al ojeto <paramref name="info"/>.
        /// </summary>
        /// <param name="info">Objeto <see cref="Mp3InfoGui"/>.</param>
        /// <param name="pattern">Patrón a utilizar o <c>null</c> para usar el seleccionado.</param>
        /// <returns><c>true</c> si la operación se lleva a cabo correctamente.</returns>
        private bool ApplyPattern(Mp3InfoGui info, ToolStripMenuItem pattern)
        {
            ToolStripMenuItem automaticItem = pattern ?? this.DetectMenuItemPattern(info);
            if (automaticItem == null)
            {
                return false;
            }

            string file = info.GetFullPath();
            string filename = Path.GetFileNameWithoutExtension(file);
            if (string.IsNullOrEmpty(filename))
            {
                return false;
            }

            if (automaticItem == this.patternDashMenuItem || this.patternDashMenuItem.Checked)
            {
                int index = filename.IndexOf("-");
                if (index > 0)
                {
                    info.Artist = filename.Substring(0, index).Trim();
                    info.Title = filename.Substring(index + 1).Trim();
                }
                else
                {
                    info.Artist = string.Empty;
                    info.Title = filename;
                }

                this.RenameCells(this.GetCurrentRenameType(), false);
                return true;
            }

            if (automaticItem == this.patternFolderToolStripMenuItem || this.patternFolderToolStripMenuItem.Checked)
            {
                DirectoryInfo directory = new DirectoryInfo(file);
                if (directory.Parent != null)
                {
                    info.Album = directory.Parent.Name;
                    info.Title = filename;

                    this.RenameCells(this.GetCurrentRenameType(), false);
                    return true;
                }
            }

            if (automaticItem == this.patternArtistAlbumFolderMenuItem || this.patternArtistAlbumFolderMenuItem.Checked)
            {                
                DirectoryInfo directory = new DirectoryInfo(file);
                string name;
                name = directory.Parent.Name;
                while (directory.Parent != null)
                {
                    name = directory.Parent.Name;
                    int index = name.IndexOf("-");
                    if (index > 0)
                    {
                        info.Album = name.Substring(index + 1).Trim();
                        info.Title = filename;
                        info.Artist = name.Substring(0, index).Trim();
                    }
                    else
                    {
                        directory = directory.Parent;
                        continue;
                    }

                    this.RenameCells(this.GetCurrentRenameType(), false);
                    return true;
                }

                info.Album = name;
                info.Title = filename;
                this.RenameCells(this.GetCurrentRenameType(), false);
                return true;
            }

            return false;
        }

        #endregion

        #region Drag & drop

        /// <summary>
        /// Invocado al arrastrar un objeto en el control.
        /// </summary>
        /// <param name="sender">Control origen del evento.</param>
        /// <param name="e">Parámetros del evento.</param>
        private void OnDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? 
                DragDropEffects.Copy :
                DragDropEffects.None;
        }

        /// <summary>
        /// Invocado al completar una operación de drag&amp;drop.
        /// </summary>
        /// <param name="sender">Control origen del evento.</param>
        /// <param name="e">Parámetros del evento.</param>
        private void OnDragDrop(object sender, DragEventArgs e)
        {
            try
            {
                Array files = (Array)e.Data.GetData(DataFormats.FileDrop);
                if (files != null)
                {
                    List<string> droppedFiles = new List<string>();
                    foreach (string file in files)
                    {
                        droppedFiles.Add(file);
                    }

                    this.BeginInvoke(
                        new Action<IEnumerable<string>>(this.OnDroppedFiles), droppedFiles);
                    this.Activate();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error in DragDrop function: " + ex.Message);
            }
        }

        /// <summary>
        /// Invocado al completar una operación de drag&amp;drop.
        /// </summary>
        /// <param name="files">Lista de archivos.</param>
        private void OnDroppedFiles(IEnumerable<string> files)
        {
            this.AddFilesAndFolders(files, this.includeSubfoldersMenuItem.Checked);
        }

        #endregion

        #region Other

        /// <summary>
        /// Invocado cuando se pulsa una tecla en el control.
        /// </summary>
        /// <param name="sender">Control origen del evento.</param>
        /// <param name="e">Parámetros del evento.</param>
        private void OnGridKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                List<int> selectedRows = this.GetSelectedRows();
                for (int i = selectedRows.Count - 1; i >= 0; i--)
                {
                    this.grid.Rows.RemoveAt(selectedRows[i]);
                }
            }
        }

        /// <summary>
        /// Selección el ítem indicado y desmarca el resto de ítems de su menú.
        /// </summary>
        /// <param name="clickedItem">Ítem a marcar.</param>
        private void CheckMenuItem(ToolStripMenuItem clickedItem)
        {
            if (clickedItem != null)
            {
                foreach (ToolStripItem subitem in clickedItem.GetCurrentParent().Items)
                {
                    ToolStripMenuItem submenu = subitem as ToolStripMenuItem;
                    if (submenu != null)
                    {
                        submenu.Checked = submenu == clickedItem;
                    }
                }
            }
        }

        /// <summary>
        /// Devuelve una lista con los índices de las filas seleccionadas.
        /// </summary>
        /// <returns>Una lista con los índices de las filas seleccionadas.</returns>
        private List<int> GetSelectedRows()
        {
            List<int> sortedList = new List<int>();
            foreach (DataGridViewCell cell in this.grid.SelectedCells)
            {
                if (!sortedList.Contains(cell.RowIndex))
                {
                    sortedList.Add(cell.RowIndex);
                }
            }

            if (sortedList.Count > 0)
            {
                sortedList.Sort();
            }

            return sortedList;
        }

        /// <summary>
        /// Invocado al finalizar una oepración de enlace de datos.
        /// </summary>
        /// <param name="sender">Control origen del evento.</param>
        /// <param name="e">Parámetros del evento.</param>
        private void OnGridDataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.addFilesLink.Visible = this.grid.Rows.Count == 0;
        }

        #endregion

        #region Add files and folders

        /// <summary>
        /// Añade todos los archivos y carpetas indicados.
        /// </summary>
        /// <param name="files">Ruta a los archivos y/o carpetas.</param>
        /// <param name="recursive"><c>true</c> para incluir las subcarpetas.</param>
        private void AddFilesAndFolders(IEnumerable<string> files, bool recursive)
        {
            bool changed = false;
            foreach (string file in files)
            {
                if (Directory.Exists(file))
                {
                    if (this.AddFolder(file, recursive))
                    {
                        changed = true;
                    }
                }
                else
                {
                    if (this.AddFile(file, null))
                    {
                        changed = true;
                    }
                }
            }

            if (changed)
            {
                this.grid.DataSource = this.selectedFiles;

                this.grid.Columns["TrackNumber"].HeaderText = Resources.TrackNumber;
                this.grid.Columns["FileName"].HeaderText = Resources.FileName;
                this.grid.Columns["Album"].HeaderText = Resources.Album;
                this.grid.Columns["Title"].HeaderText = Resources.Title;
                this.grid.Columns["Artist"].HeaderText = Resources.Artist;
                this.grid.Columns["Path"].HeaderText = Resources.Path;
            }
        }

        /// <summary>
        /// Añade todos los archivos de la carpeta indicada.
        /// </summary>
        /// <param name="folder">Ruta a la carpeta.</param>
        /// <param name="recursive"><c>true</c> para incluir las subcarpetas.</param>
        /// <returns><c>true</c> si se ha añadido el archivo.</returns>
        private bool AddFolder(string folder, bool recursive)
        {
            SearchOption option = 
                recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            string[] files = Directory.GetFiles(folder, "*.mp3", option);

            // En modo automático hago una pasada previa para ver cual es el patrón más frecuente. 
            // De este modo, se aplica el mismo patrón a todos los archivos de la carpeta aunque 
            // alguno suelto pueda tener otro patrón distinto
            ToolStripMenuItem patternMenuItem = null;
            if (this.patternAutomaticMenuItem.Checked && files.Length > 0)
            {
                int max = 0;
                Dictionary<ToolStripMenuItem, int> patternFrecuency = new Dictionary<ToolStripMenuItem, int>();
                foreach (string file in files)
                {
                    ToolStripMenuItem automaticItem = this.DetectMenuItemPattern(new Mp3InfoGui(file));
                    if (automaticItem != null)
                    {
                        if (patternFrecuency.ContainsKey(automaticItem))
                        {
                            patternFrecuency[automaticItem]++;
                        }
                        else
                        {
                            patternFrecuency.Add(automaticItem, 1);
                        }

                        if (patternFrecuency[automaticItem] > max)
                        {
                            max = patternFrecuency[automaticItem];
                            patternMenuItem = automaticItem;
                        }
                    }
                }
            }

            return this.AddFiles(files, patternMenuItem);
        }

        /// <summary>
        /// Añade los archivos indicados.
        /// </summary>
        /// <param name="files">Ruta a los archivos.</param>
        /// <param name="pattern">Patrón a utilizar o <c>null</c> para usar el seleccionado.</param>
        /// <returns><c>true</c> si se ha añadido el archivo.</returns>
        private bool AddFiles(IEnumerable<string> files, ToolStripMenuItem pattern)
        {
            bool changed = false;
            foreach (string file in files)
            {
                if (this.AddFile(file, pattern))
                {
                    changed = true;
                }
            }

            return changed;
        }

        /// <summary>
        /// Añade el archivo indicado.
        /// </summary>
        /// <param name="file">Ruta al archivo.</param>
        /// <param name="pattern">Patrón a utilizar o <c>null</c> para usar el seleccionado.</param>
        /// <returns><c>true</c> si se ha añadido el archivo.</returns>
        private bool AddFile(string file, ToolStripMenuItem pattern)
        {
            bool found = false;
            foreach (Mp3InfoGui info in this.selectedFiles)
            {
                if (string.Compare(info.GetFullPath(), file, true) == 0)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Mp3InfoGui info = this.CreateMp3Info(file, pattern);
                if (info != null)
                {
                    this.selectedFiles.Add(info);
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Menu operations

        /// <summary>
        /// Seleccionar carpeta.
        /// </summary>
        /// <param name="sender">Control origen del evento.</param>
        /// <param name="e">Parámetros del evento.</param>
        private void OnSelectFolderMenuItemClick(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    List<string> folders = new List<string> { folderDialog.SelectedPath };
                    this.AddFilesAndFolders(folders, this.includeSubfoldersMenuItem.Checked);
                }
            }
        }

        /// <summary>
        /// Seleccionar archivos.
        /// </summary>
        /// <param name="sender">Control origen del evento.</param>
        /// <param name="e">Parámetros del evento.</param>
        private void OnSelectFilesMenuItemClick(object sender, EventArgs e)
        {
            using (OpenFileDialog openDialog = new OpenFileDialog
                {
                    DefaultExt = "mp3",
                    Filter = Resources.AudioFilesFilter,
                    Multiselect = true
                })
            {
                DialogResult result = openDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.AddFilesAndFolders(openDialog.FileNames, this.includeSubfoldersMenuItem.Checked);
                }
            }
        }

        /// <summary>
        /// Guarda los cambios.
        /// </summary>
        /// <param name="sender">Control origen del evento.</param>
        /// <param name="e">Parámetros del evento.</param>
        private void OnSaveMenuItemClick(object sender, EventArgs e)
        {
            int count = this.grid.Rows.Count;
            for (int i = this.grid.Rows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow row = this.grid.Rows[i];
                Mp3InfoGui info = row.DataBoundItem as Mp3InfoGui;
                if (info == null)
                {
                    continue;
                }

                ID3Info infoFile = new ID3Info(info.GetFullPath(), true);

                try
                {
                    // Renombro el ID3 del archivo
                    infoFile.ID3v1Info.Album =
                        info.Album != null && info.Album.Length > 30 ? info.Album.Substring(0, 30) : info.Album;
                    infoFile.ID3v1Info.Title =
                        info.Title != null && info.Title.Length > 30 ? info.Title.Substring(0, 30) : info.Title;
                    infoFile.ID3v1Info.Artist =
                        info.Artist != null && info.Artist.Length > 30 ? info.Artist.Substring(0, 30) : info.Artist;
                    if (!String.IsNullOrEmpty(info.TrackNumber))
                    {
                        int AuxTrackNumber = Convert.ToInt32(info.TrackNumber);
                        infoFile.ID3v1Info.TrackNumber = (byte) AuxTrackNumber;
                    }
                    infoFile.ID3v1Info.HaveTag = true;

                    infoFile.ID3v2Info.SetTextFrame("TALB", info.Album);
                    infoFile.ID3v2Info.SetTextFrame("TIT2", info.Title);
                    infoFile.ID3v2Info.SetTextFrame("TPE1", info.Artist);
                    if (!String.IsNullOrEmpty(info.TrackNumber))                    
                        infoFile.ID3v2Info.SetTextFrame("TRCK", info.TrackNumber);

                    infoFile.ID3v2Info.HaveTag = true;

                    infoFile.Save();

                    // Si ha cambiado, renombro el archivo en si
                    string name = Path.GetFileNameWithoutExtension(info.GetFullPath());
                    if (name != null && string.Compare(name, info.FileName, false) != 0)
                    {
                        FileInfo fileInfo = new FileInfo(info.GetFullPath());
                        name = Path.Combine(fileInfo.Directory.FullName, info.FileName) + fileInfo.Extension;
                        File.Move(info.GetFullPath(), name);
                    }

                    this.grid.Rows.RemoveAt(i);
                }
                catch (Exception)
                {
                }
            }

            int errorCount = this.grid.Rows.Count;
            this.statusLabel.Text = errorCount == 0 ? 
                string.Format(Resources.RenamedXItems, count - errorCount) :
                string.Format(Resources.RenamedXItemsWithErrors, count - errorCount, errorCount);
        }

        /// <summary>
        /// Invocado para limpiar la lista de archivos.
        /// </summary>
        /// <param name="sender">Control origen del evento.</param>
        /// <param name="e">Parámetros del evento.</param>
        private void OnCleanListMenuItemClick(object sender, EventArgs e)
        {
            this.grid.Rows.Clear();
        }

        /// <summary>
        /// Cerrar ventana.
        /// </summary>
        /// <param name="sender">Control origen del evento.</param>
        /// <param name="e">Parámetros del evento.</param>
        private void OnExitMenuItemClick(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Copiar selección.
        /// </summary>
        /// <param name="sender">Control origen del evento.</param>
        /// <param name="e">Parámetros del evento.</param>
        private void OnCopyMenuItemClick(object sender, EventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            List<int> selectedRows = this.GetSelectedRows();
            foreach (int rowIndex in selectedRows)
            {
                Mp3InfoGui info = this.grid.Rows[rowIndex].DataBoundItem as Mp3InfoGui;
                if (info != null)
                {
                    builder.AppendFormat(
                        "{0}\t{1}\t{2}\t{3}\r\n", info.FileName, info.Album, info.Title, info.Path);
                }
            }

            Clipboard.SetData(DataFormats.Text, builder.ToString());
        }

        /// <summary>
        /// Pegar texto copiado.
        /// </summary>
        /// <param name="sender">Control origen del evento.</param>
        /// <param name="e">Parámetros del evento.</param>
        private void OnPasteMenuItemClick(object sender, EventArgs e)
        {
            if (!Clipboard.ContainsText())
            {
                return;
            }

            List<int> selectedRows = this.GetSelectedRows();
            if (selectedRows.Count == 0)
            {
                return;
            }

            bool refresh = false;
            string text = Clipboard.GetText().Replace("\r\n", "\n");
            string[] lines = text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 1)
            {
                string[] parts = lines[0].Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2)
                {
                    int startIndex = parts.Length > 2 ? 1 : 0;
                    foreach (int rowIndex in selectedRows)
                    {
                        Mp3InfoGui info = this.grid.Rows[rowIndex].DataBoundItem as Mp3InfoGui;
                        if (info != null)
                        {
                            info.Album = parts[startIndex];
                            info.Title = parts[startIndex + 1];
                            refresh = true;
                        }
                    }
                }
            }
            else
            {
                // Si no hay nada seleccionado pero la selección coincide con el tamaño del grid, pego en el grid
                if (selectedRows.Count == 1 && lines.Length != 1 && lines.Length == this.grid.Rows.Count)
                {
                    selectedRows.Clear();
                    for (int i = 0; i < this.grid.Rows.Count; i++)
                    {
                        selectedRows.Add(i);
                    }
                }

                if (selectedRows.Count == lines.Length)
                {
                    int lineIndex = 0;
                    foreach (int rowIndex in selectedRows)
                    {
                        string[] parts = lines[lineIndex++].Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 2)
                        {
                            int startIndex = parts.Length > 2 ? 1 : 0;
                            Mp3InfoGui info = this.grid.Rows[rowIndex].DataBoundItem as Mp3InfoGui;
                            if (info != null)
                            {
                                info.Album = parts[startIndex];
                                info.Title = parts[startIndex + 1];
                                refresh = true;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(Resources.PasteNoMatchLineCount, this.Text);
                }
            }

            if (refresh)
            {
                this.grid.Refresh();
            }
        }

        /// <summary>
        /// Seleccionar todas las filas.
        /// </summary>
        /// <param name="sender">Control origen del evento.</param>
        /// <param name="e">Parámetros del evento.</param>
        private void OnSelectAllMenuItemClick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.grid.Rows)
            {
                row.Selected = true;
            }
        }

        /// <summary>
        /// Invertir selección.
        /// </summary>
        /// <param name="sender">Control origen del evento.</param>
        /// <param name="e">Parámetros del evento.</param>
        private void OnInvertSelectionMenuItemClick(object sender, EventArgs e)
        {
            List<int> selectedRows = this.GetSelectedRows();
            for (int row = 0; row < this.grid.Rows.Count; row++)
            {
                this.grid.Rows[row].Selected = !selectedRows.Contains(row);
            }
        }

        /// <summary>
        /// Incluir o no los subdirectorios en las operaciones.
        /// </summary>
        /// <param name="sender">Control origen del evento.</param>
        /// <param name="e">Parámetros del evento.</param>
        private void OnIncludeSubfoldersMenuItemClick(object sender, EventArgs e)
        {
            this.includeSubfoldersMenuItem.Checked = !this.includeSubfoldersMenuItem.Checked;
        }

        /// <summary>
        /// Aplicar o no excepciones en algunos tipos de renombrado.
        /// </summary>
        /// <param name="sender">Control origen del evento.</param>
        /// <param name="e">Parámetros del evento.</param>
        private void OnIntelligentConversionMenuItemClick(object sender, EventArgs e)
        {
            this.intelligentConversionMenuItem.Checked = !this.intelligentConversionMenuItem.Checked;
        }

        /// <summary>
        /// Eliminar o no los números de canción.
        /// </summary>
        /// <param name="sender">Control origen del evento.</param>
        /// <param name="e">Parámetros del evento.</param>
        private void OnRemoveTrackNumberMenuItemClick(object sender, EventArgs e)
        {
            this.removeTrackNumberMenuItem.Checked = !this.removeTrackNumberMenuItem.Checked;
        }

        /// <summary>
        /// Invocado al pulsar el enlace de añadir archivos.
        /// </summary>
        /// <param name="sender">Control origen del evento.</param>
        /// <param name="e">Parámetros del evento.</param>
        private void OnAddFilesLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.OnSelectFilesMenuItemClick(sender, EventArgs.Empty);
        }

        #endregion

        #region Case operations

        /// <summary>
        /// Renombra las celdas del grid.
        /// </summary>
        /// <param name="sender">Control origen del evento.</param>
        /// <param name="e">Parámetros del evento.</param>
        private void OnRenameCellsMenuItemClick(object sender, EventArgs e)
        {
            this.CheckMenuItem(sender as ToolStripMenuItem);

            // Si solo hay una celda seleccionada y no es editable, lo aplico a todas las filas
            bool applyToAll = this.grid.SelectedCells.Count == 1 && this.grid.SelectedCells[0].ReadOnly;
            this.RenameCells(this.GetCurrentRenameType(), !applyToAll);
        }

        /// <summary>
        /// Renombra las celdas del grid usando el patrón "Álbum - Título".
        /// </summary>
        /// <param name="sender">Control origen del evento.</param>
        /// <param name="e">Parámetros del evento.</param>
        private void OnPatternMenuItemClick(object sender, EventArgs e)
        {
            this.CheckMenuItem(sender as ToolStripMenuItem);

            // La operación se aplica a todas las filas donde haya celdas seleccionadas
            List<int> selectedRows = this.GetSelectedRows();
            foreach (int rowIndex in selectedRows)
            {
                Mp3InfoGui info = this.grid.Rows[rowIndex].DataBoundItem as Mp3InfoGui;
                if (info != null && this.ApplyPattern(info, sender as ToolStripMenuItem))
                {
                    this.grid.Refresh();
                }
            }
        }

        #endregion
    }
}
