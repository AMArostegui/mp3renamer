using System.ComponentModel;
using System.Text;

namespace Test_AM
{
    /// <summary>
    /// Informarción de archivo MP3 relevante para la interfaz de usuario.
    /// </summary>
    public class Mp3InfoGui
    {
        /// <summary>
        /// Ruta completa al archivo.
        /// </summary>
        private readonly string fullPath;

        /// <summary>
        /// Nombre del álbum.
        /// </summary>
        private string album;

        /// <summary>
        /// Título de la canción.
        /// </summary>
        private string title;

        /// <summary>
        /// Artista de la canción.
        /// </summary>
        private string artist;

        /// <summary>
        /// Inicializa una instancia de la clase <see cref="Mp3InfoGui"/>.
        /// </summary>
        /// <param name="file">Ruta al archivo asociado.</param>
        public Mp3InfoGui(string file)
        {
            // Campitos que se sacan del nombre de archivo
            this.fullPath = file;
            this.FileName = System.IO.Path.GetFileNameWithoutExtension(file);
            this.Path = System.IO.Path.GetDirectoryName(file);

            // Sacar número de pista del nombre de archivo si procede            
            StringBuilder trackAux = new StringBuilder();
            int i = 0;
            while (this.FileName[i] >= '0' && this.FileName[i] <= '9' && i < this.FileName.Length)
            {
                trackAux.Append(this.FileName[i]);
                i++;
            }
            int AuxTrackNumber;
            if (int.TryParse(trackAux.ToString(), out AuxTrackNumber))
                TrackNumber = AuxTrackNumber.ToString();

            this.album = string.Empty;
            this.title = string.Empty;
            this.artist = string.Empty;
        }

        /// <summary>
        /// Número de pista.
        /// </summary>
        [Bindable(true)]
        public string TrackNumber { get; set; }

        /// <summary>
        /// Nombre del archivo sin extensión.
        /// </summary>
        [Bindable(true)]
        public string FileName { get; set; }

        /// <summary>
        /// Nombre del álbum.
        /// </summary>
        [Bindable(true)]
        public string Album
        {
            get { return this.album; }
            set { this.album = SetValue(value); }
        }

        /// <summary>
        /// Título de la canción.
        /// </summary>
        [Bindable(true)]
        public string Title
        {
            get { return this.title; }
            set { this.title = SetValue(value); }
        }

        /// <summary>
        /// Artista de la canción.
        /// </summary>
        [Bindable(true)]
        public string Artist
        {
            get { return this.artist; }
            set { this.artist = SetValue(value); }
        }

        /// <summary>
        /// Ruta completa del archivo.
        /// </summary>
        [Bindable(true)]
        public string Path { get; private set; }

        /// <summary>
        /// Devuelve la ruta completa al archivo.
        /// </summary>
        /// <returns>La ruta completa al archivo.</returns>
        public string GetFullPath()
        {
            return this.fullPath;
        }

        /// <summary>
        /// Devuelve el valor indicado en <paramref name="value"/> salvo si es <c>null</c>, que 
        /// devuelve cadena vacía.
        /// </summary>
        /// <param name="value">Valor asociado.</param>
        /// <returns>El valor indicado en <paramref name="value"/> o cadena vacía si es <c>null</c>.
        /// </returns>
        private static string SetValue(string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : value;
        }
    }
}
